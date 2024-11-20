using LicenseeRecords.Web.Interfaces;
using LicenseeRecords.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LicenseeRecords.Web.Controllers;
public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;
    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetProducts();
        return View(products);
    }

    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            await _productService.Post(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int productId)
    {
        var product = await _productService.GetProductById(productId);
        if (product == null)
            return View("Error");
        return View(product);
    }

    [HttpPost, ActionName("Update")]
    public async Task<IActionResult> UpdateProduct(int productId, Product updatedProduct)
    {
        if (!ModelState.IsValid)
        {
            return View(updatedProduct);
        }

        var product = await _productService.GetProductById(productId);
        if (product == null)
            return View("Error");

        await _productService.Put(updatedProduct);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int productId)
    {
        var product = await _productService.GetProductById(productId);
        if (product == null)
            return View("Error");
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var product = await _productService.GetProductById(productId);
        if (product == null)
            return View("Error");

        await _productService.Delete(productId);
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