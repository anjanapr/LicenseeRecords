using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LicenseeRecords.Web.Interfaces;
using LicenseeRecords.Web.Models;

namespace LicenseeRecords.Web.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Account>> GetAccounts()
    {
        try
        {
            var result = await _httpClient.GetAsync("account");
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<List<Account>>() ?? new List<Account>();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve accounts", ex);
        }
    }

    public async Task<Account?> GetAccountById(int accountId)
    {
        try
        {
            var result = await _httpClient.GetAsync($"account/{accountId}");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Account>();
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to retrieve account with id {accountId}", ex);
        }
    }

    public async Task Post(Account account)
    {
        var result = await _httpClient.PostAsJsonAsync("account", account);
        if (!result.IsSuccessStatusCode)
        {
            var response = await result.Content.ReadAsStringAsync();
            throw new Exception($"API Error: {result.StatusCode}, Details: {response}");
        }
        result.EnsureSuccessStatusCode();
    }

    public async Task Put(Account account)
    {
        try
        {
            var result = await _httpClient.PutAsJsonAsync($"account/{account.AccountId}", account);
            result.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update account", ex);
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            var result = await _httpClient.DeleteAsync($"account/{id}");
            result.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete account", ex);
        }
    }
}