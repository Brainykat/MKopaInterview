using Common.Shared.Services;
using Customers.Domain.Entities;
using Customers.Services.Interfaces;
using Customers.ViewsAndDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Customers.API.Controllers
{
  
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class CustomersController : ControllerBase
  {
    //Controllers should be pass through only
    //Abstract Custom Authorization and Authentication too
    private readonly ICustomerService service;
    private readonly ILogger<CustomersController> logger;

    [HttpGet]
    [ProducesResponseType(typeof(ICollection<Customer>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Customers() => Ok(await service.GetCustomers());
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Customer(Guid id) => Ok(await service.GetCustomer(id));
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Add(CustomerDto dto)
    {
      try
      {
        return StatusCode(await service.Add(dto));
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return StatusCode(500, ex.Message);
      }
    }
    [HttpPost("{id}")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(Guid id, CustomerDto dto)
    {
      try
      {
        return StatusCode(await service.Add(dto));
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return StatusCode(500, ex.Message);
      }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      try
      {
        return StatusCode(await service.Delete(id));
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return StatusCode(500, ex.Message);
      }
    }
  }
}
