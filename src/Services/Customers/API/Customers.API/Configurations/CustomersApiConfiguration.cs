using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.API.Configurations
{
  public class CustomersApiConfiguration
  {
    public string ApiName { get; set; }
    public string ApiVersion { get; set; }
    public string IdentityServerBaseUrl { get; set; }
    public string ApiBaseUrl { get; set; }
    public string OidcSwaggerUIClientId { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public string OidcApiName { get; set; }
    public string AdministrationRole { get; set; }
    public bool CorsAllowAnyOrigin { get; set; }
    public string Secrete { get; set; }
    public string[] CorsAllowOrigins { get; set; }
  }
}
