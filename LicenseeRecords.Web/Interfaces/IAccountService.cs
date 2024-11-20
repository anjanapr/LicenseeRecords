using LicenseeRecords.Web.Models;

namespace LicenseeRecords.Web.Interfaces;

public interface IAccountService
{
    Task<List<Account>> GetAccounts();
    Task<Account> GetAccountById(int accountId);
    Task Post(Account account);
    Task Put(Account account);
    Task Delete(int id);
}