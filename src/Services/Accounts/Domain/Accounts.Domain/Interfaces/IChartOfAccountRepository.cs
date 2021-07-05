using Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounts.Domain.Interfaces
{
  public interface IChartOfAccountRepository
  {
    Task Add(ChartOfAccount chart);
    Task<ChartOfAccount> FindChartOfAccount(string name);
    Task<ChartOfAccount> GetChartOfAccount(Guid id);
    Task<ChartOfAccount> GetChartOfAccount(int number);
    Task<List<ChartOfAccount>> GetChartOfAccounts();
    Task<List<ChartOfAccount>> GetCustomerCOAs();
    Task<List<ChartOfAccount>> GetOfficeCOAs();
  }
}
