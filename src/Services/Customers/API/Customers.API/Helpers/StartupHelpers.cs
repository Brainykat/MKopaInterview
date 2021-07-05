using Customers.API.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.API.Helpers
{
  public static class StartupHelpers
  {
    public static IServiceCollection AddAdminApiCors(this IServiceCollection services, CustomersApiConfiguration clientsgApiConfiguration)
    {
      services.AddCors(options =>
      {
        options.AddDefaultPolicy(
            builder =>
            {
              if (clientsgApiConfiguration.CorsAllowAnyOrigin)
              {
                builder.AllowAnyOrigin();
              }
              else
              {
                builder.WithOrigins(clientsgApiConfiguration.CorsAllowOrigins);
              }

              builder.AllowAnyHeader();
              builder.AllowAnyMethod();
            });
      });

      return services;
    }
    public static void AddForwardHeaders(this IApplicationBuilder app)
    {
      var forwardingOptions = new ForwardedHeadersOptions()
      {
        ForwardedHeaders = ForwardedHeaders.All
      };
      forwardingOptions.KnownNetworks.Clear();
      forwardingOptions.KnownProxies.Clear();
      app.UseForwardedHeaders(forwardingOptions);
    }
  }
}
