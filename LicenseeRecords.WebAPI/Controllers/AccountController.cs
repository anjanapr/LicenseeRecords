using LicenseeRecords.WebAPI.Interfaces;
using LicenseeRecords.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LicenseeRecords.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;
    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Account>))]
    public async Task<IActionResult> GetAccounts()
    {
        var accounts = await _accountRepository.GetAllAccounts();
        return Ok(accounts);
    }

    [HttpGet("{accountId}", Name = "GetAccount")]
    [ProducesResponseType(200, Type = typeof(Account))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAccount(int accountId)
    {
        var account = await _accountRepository.GetAccountById(accountId);
        if (account is null)
            return NotFound();
        return Ok(account);
    }

    [HttpPost()]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Account>> Create(Account account)
    {
        if (account is null)
            return BadRequest(ModelState);

        var newAccount = await _accountRepository.Create(account);
        return CreatedAtRoute(nameof(GetAccount), new { accountId = newAccount.AccountId }, newAccount);
    }

    [HttpPut("{accountId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int accountId, Account updatedAccount)
    {
        if (updatedAccount is null)
            return BadRequest(ModelState);

        if (accountId != updatedAccount.AccountId)
            return BadRequest(ModelState);

        var accountToUpdate = await _accountRepository.GetAccountById(accountId);
        if (accountToUpdate is null)
            return NotFound();

        await _accountRepository.Update(updatedAccount);
        return NoContent();
    }

    [HttpDelete("{accountId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int accountId)
    {
        var accountToDelete = await _accountRepository.GetAccountById(accountId);
        if (accountToDelete is null)
            return NotFound();

        await _accountRepository.Delete(accountToDelete.AccountId);
        return NoContent();
    }
}