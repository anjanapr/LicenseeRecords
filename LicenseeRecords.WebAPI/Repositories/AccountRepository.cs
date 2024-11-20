using System.Text.Json;
using System.Text.Json.Serialization;
using LicenseeRecords.WebAPI.Interfaces;
using LicenseeRecords.WebAPI.Models;

namespace LicenseeRecords.WebAPI.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly string _accountsFilePath;
    public AccountRepository(string filePath)
    {
        _accountsFilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    private async Task<List<Account>> ReadFromFile()
    {
        try
        {
            if (!File.Exists(_accountsFilePath))
            {
                return new List<Account>();
            }
            var json = await File.ReadAllTextAsync(_accountsFilePath);
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<Account>>(json, options) ?? new List<Account>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading from file", ex);
        }
    }

    private async Task SaveToFile(List<Account> accounts)
    {
        try
        {
            var json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_accountsFilePath, json);
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving to file", ex);
        }
    }

    public async Task<IEnumerable<Account>> GetAllAccounts()
    {
        return await ReadFromFile();
    }

    public async Task<Account?> GetAccountById(int id)
    {
        return (await ReadFromFile()).FirstOrDefault(item => item.AccountId == id);
    }

    public async Task<Account> Create(Account account)
    {
        var accounts = await ReadFromFile();
        account.AccountId = accounts.Any() ? accounts.Count + 1 : 1;
        accounts.Add(account);
        await SaveToFile(accounts);
        return account;
    }

    public async Task Update(Account account)
    {
        var accounts = await ReadFromFile();
        var existingAccount = accounts.FirstOrDefault(item => item.AccountId == account.AccountId);
        if (existingAccount == null)
            throw new KeyNotFoundException($"Account with id {account.AccountId} not found");

        existingAccount.AccountName = account.AccountName;
        existingAccount.AccountStatus = account.AccountStatus;
        existingAccount.ProductLicence = account.ProductLicence;
        await SaveToFile(accounts);
    }

    public async Task Delete(int id)
    {
        var accounts = await ReadFromFile();
        var accountToDelete = accounts.FirstOrDefault(item => item.AccountId == id);
        if (accountToDelete == null)
            throw new KeyNotFoundException($"Account with id {id} not found");

        accounts.Remove(accountToDelete);
        await SaveToFile(accounts);
    }
}