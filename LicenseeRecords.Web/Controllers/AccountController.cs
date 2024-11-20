using LicenseeRecords.Web.Interfaces;
using LicenseeRecords.Web.Models;
using LicenseeRecords.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LicenseeRecords.Web.Controllers;
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    private readonly IProductService _productService;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService,
        IProductService productService)
    {
        _logger = logger;
        _accountService = accountService;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accounts = await _accountService.GetAccounts();
        return View(accounts);
    }

    [HttpGet]
    public async Task<IActionResult> SaveAccountName(Account account)
    {
        if (account.AccountId != 0)
        {
            var existingAccount = await _accountService.GetAccountById(account.AccountId);
            if (existingAccount == null)
                return View("Error");
            return View(existingAccount);
        }
        return View(account);
    }

    [HttpPost]
    public async Task<IActionResult> SaveName(Account account)
    {
        if (account.AccountId == 0)
        {
            return RedirectToAction("SaveAccountStatus", new
            {
                accountId = account.AccountId,
                accountName = account.AccountName
            });
        }
        else
        {
            var existingAccount = await _accountService.GetAccountById(account.AccountId);
            existingAccount.AccountName = account.AccountName;
            await _accountService.Put(existingAccount);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> SaveAccountStatus(Account account)
    {
        if (account.AccountId != 0)
        {
            var existingAccount = await _accountService.GetAccountById(account.AccountId);
            if (existingAccount == null)
                return View("Error");
            return View(existingAccount);
        }

        return View(account);
    }

    [HttpPost]
    public async Task<IActionResult> SaveStatus(Account account)
    {
        if (account.AccountId == 0)
        {
            return RedirectToAction("SaveProductLicence", new
            {
                accountId = account.AccountId,
                accountName = account.AccountName,
                accountStatus = account.AccountStatus
            });
        }
        else
        {
            var existingAccount = await _accountService.GetAccountById(account.AccountId);
            existingAccount.AccountStatus = account.AccountStatus;
            await _accountService.Put(existingAccount);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> SaveProductLicence(Account account)
    {
        var products = await _productService.GetProducts();
        var productLicenceViewModel = new ProductLicenceViewModel
        {
            AccountId = account.AccountId,
            AccountName = account.AccountName,
            AccountStatus = account.AccountStatus,
            ProductLicence = new ProductLicence(),
            Products = products
        };

        return View(productLicenceViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAccount(int accountId, int? StartDay, int? StartMonth,
        int? StartYear, int? EndDay, int? EndMonth,
        int? EndYear, ProductLicenceViewModel updatedProductLicence)
    {
        var account = await _accountService.GetAccountById(accountId);
        if (StartDay.HasValue && StartMonth.HasValue && StartYear.HasValue)
            updatedProductLicence.ProductLicence.LicenceFromDate = new DateTime(StartYear.Value, StartMonth.Value, StartDay.Value);
        else
            updatedProductLicence.ProductLicence.LicenceFromDate = null;
        if (EndDay.HasValue && EndMonth.HasValue && EndYear.HasValue)
            updatedProductLicence.ProductLicence.LicenceToDate = new DateTime(EndYear.Value, EndMonth.Value, EndDay.Value);
        else
            updatedProductLicence.ProductLicence.LicenceToDate = null;

        var selectedProduct = await _productService.GetProductById(updatedProductLicence.ProductLicence.Product.ProductId);

        var newProductLicence = new ProductLicence
        {
            LicenceId = (account != null) ? account.ProductLicence.Count + 1 : 1,
            LicenceStatus = updatedProductLicence.ProductLicence.LicenceStatus,
            LicenceFromDate = updatedProductLicence.ProductLicence.LicenceFromDate,
            LicenceToDate = updatedProductLicence.ProductLicence.LicenceToDate,
            Product = selectedProduct
        };
        if (accountId == 0)
        {
            var newAccount = new Account
            {
                AccountId = updatedProductLicence.AccountId,
                AccountName = updatedProductLicence.AccountName,
                AccountStatus = updatedProductLicence.AccountStatus
            };
            newAccount.ProductLicence.Add(newProductLicence);
            await _accountService.Post(newAccount);
        }
        else
        {
            account?.ProductLicence.Add(newProductLicence);
            await _accountService.Put(account);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteProductLicence(int accountId, int licenceId)
    {
        var account = await _accountService.GetAccountById(accountId);
        if (account == null)
            return View("Error");

        var productLicence = account.ProductLicence.FirstOrDefault(item => item.LicenceId == licenceId);

        var productLicenceViewModel = new ProductLicenceViewModel
        {
            AccountId = account.AccountId,
            ProductLicence = productLicence,
            Products = null
        };
        return View(productLicenceViewModel);
    }

    [HttpPost, ActionName("DeleteProductLicence")]
    public async Task<IActionResult> DeleteLicence(int accountId, int licenceId)
    {
        var account = await _accountService.GetAccountById(accountId);
        if (account == null)
            return View("Error");
        var licenceToDelete = account.ProductLicence.FirstOrDefault(item => item.LicenceId == licenceId);
        account.ProductLicence.Remove(licenceToDelete);
        await _accountService.Put(account);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}