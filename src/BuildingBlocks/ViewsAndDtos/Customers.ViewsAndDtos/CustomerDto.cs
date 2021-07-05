using System;

namespace Customers.ViewsAndDtos
{
  public class CustomerDto
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string IdNumber { get; set; }
    public string PhoneNumber { get; set; }
  }
}
