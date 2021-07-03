using Common.Base.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Domain.Entities
{
  public class Customer : EntityBase
  {
    public static Customer Create(string firstName, string lastName, DateTime dateOfBirth, string idNumber,
      string phoneNumber) => new Customer(firstName, lastName, dateOfBirth, idNumber, phoneNumber);
    private Customer(string firstName, string lastName, DateTime dateOfBirth, string idNumber, 
      string phoneNumber)
    {
      FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
      LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
      DateOfBirth = dateOfBirth;
      IdNumber = idNumber ?? throw new ArgumentNullException(nameof(idNumber));
      PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
      GenerateNewIdentity();
    }
    private Customer() { }
    public string FirstName { get; set; }
   public string LastName {get;set;}
   public DateTime DateOfBirth {get;set;}
   public string IdNumber {get;set;}
   public string PhoneNumber {get;set;}
  }
}
