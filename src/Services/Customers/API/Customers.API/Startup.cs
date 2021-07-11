using Customers.API.Configurations;
using Customers.API.Helpers;
using Customers.Data;
using Customers.Data.Repositories;
using Customers.Domain.Interfaces;
using Customers.Services.EBServices;
using Customers.Services.Interfaces;
using Customers.Services.Services;
using EventBus.Common.Shared.Configurations;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Customers.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var customersApiConfiguration = Configuration.GetSection(nameof(CustomersApiConfiguration)).Get<CustomersApiConfiguration>();
      services.AddSingleton(customersApiConfiguration);

      services.AddDbContext<CustomerContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("CustomersDbConnection"),
       providerOptions =>
       {
         providerOptions.EnableRetryOnFailure().CommandTimeout((int)TimeSpan.FromMinutes(0.5).TotalSeconds);
         providerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
       }));
      services.AddCustomHealthCheck(Configuration);
      services.AddTransient<ICustomerRepo, CustomerRepo>();
      services.AddTransient<ICustomerService, CustomerService>();
      services.AddTransient<ICustomerEventProducer, CustomerEventProducer>();

      services.AddAdminApiCors(customersApiConfiguration);
      services.AddControllers()
        .AddNewtonsoftJson(x =>
        {
          x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
          x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
          x.SerializerSettings.Converters.Add(new StringEnumConverter());
        });
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = customersApiConfiguration.ApiName, Version = customersApiConfiguration.ApiVersion });
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
          Type = SecuritySchemeType.OAuth2,
          Flows = new OpenApiOAuthFlows
          {
            AuthorizationCode = new OpenApiOAuthFlow
            {
              AuthorizationUrl = new Uri($"{customersApiConfiguration.IdentityServerBaseUrl}/connect/authorize"),
              TokenUrl = new Uri($"{customersApiConfiguration.IdentityServerBaseUrl}/connect/token"),
              Scopes = new Dictionary<string, string>
            {
                { "CustomerAPIScope", customersApiConfiguration.ApiName }
            }
            }
          }
        });
        //options.OperationFilter<AuthorizeCheckOperationFilter>();
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CustomersApiConfiguration clientsgApiConfiguration)
    {
      app.AddForwardHeaders();
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint($"{clientsgApiConfiguration.ApiBaseUrl}/swagger/v1/swagger.json", clientsgApiConfiguration.ApiName);
        c.OAuthClientId(clientsgApiConfiguration.OidcSwaggerUIClientId);
        c.OAuthAppName(clientsgApiConfiguration.ApiName);
        c.OAuthUsePkce();
      });

      app.UseRouting();
      app.UseCors();
      //app.UseHealthAndMetricsMiddleware();
      app.UseSerilogRequestLogging();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
        {
          Predicate = _ => true,
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
        {
          Predicate = r => r.Name.Contains("self")
        });
      });
    }
  }
  public static class CustomExtensionMethods
  {
    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
      var hcBuilder = services.AddHealthChecks();
      hcBuilder
          .AddCheck("self", () => HealthCheckResult.Healthy())
          .AddSqlServer(
              configuration.GetConnectionString("CustomersDbConnection"),
              name: "M-Kopa_Customers_DB-check",
              tags: new string[] { "Customers_db" });
      //Rabbit hc
      var rabbitMQConfiguration = configuration.GetSection(nameof(RabbitMQConfiguration)).Get<RabbitMQConfiguration>();
      hcBuilder
          .AddRabbitMQ(
              $"amqp://{rabbitMQConfiguration.HostName}",
              name: $"{rabbitMQConfiguration.HostName} catalog-rabbit MQ bus-check",
              tags: new string[] { rabbitMQConfiguration.HostName, rabbitMQConfiguration.UserName, rabbitMQConfiguration.Password });
      
      return services;
    }
  }
}
