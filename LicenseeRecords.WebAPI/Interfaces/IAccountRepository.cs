using LicenseeRecords.WebAPI.Models;

namespace LicenseeRecords.WebAPI.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAllAccounts();
    Task<Account?> GetAccountById(int id);
    Task<Account> Create(Account account);
    Task Update(Account account);
    Task Delete(int id);
}