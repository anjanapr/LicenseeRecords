using LicenseeRecords.WebAPI.Interfaces;
using LicenseeRecords.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LicenseeRecords.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productRepository.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{productId:int}", Name = "GetProduct")]
    [ProducesResponseType(200, Type = typeof(Product))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("{productName}", Name = "GetProductByName")]
    [ProducesResponseType(200, Type = typeof(Product))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProductByName(string productName)
    {
        var product = await _productRepository.GetProductByName(productName);
        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost()]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        ArgumentNullException.ThrowIfNull(product, nameof(product));

        var products = _productRepository.GetProductByName(product.ProductName).Result;

        if (products != null)
        {
            ModelState.AddModelError("", "Product already exists");
            return StatusCode(422, ModelState);
        }

        var newProduct = await _productRepository.Create(product);
        return CreatedAtRoute(nameof(GetProduct), new { productId = newProduct.ProductId }, newProduct);
    }

    [HttpPut("{productId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int productId, Product updatedProduct)
    {
        ArgumentNullException.ThrowIfNull(updatedProduct, nameof(updatedProduct));

        if (productId != updatedProduct.ProductId)
            return BadRequest(ModelState);

        var products = _productRepository.GetProductByName(updatedProduct.ProductName).Result;

        if (products != null)
        {
            ModelState.AddModelError("", "Product already exists");
            return StatusCode(422, ModelState);
        }

        var productToUpdate = await _productRepository.GetProductById(productId);
        if (productToUpdate is null)
            return NotFound();

        await _productRepository.Update(updatedProduct);
        return NoContent();
    }

    [HttpDelete("{productId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int productId)
    {
        var productToDelete = await _productRepository.GetProductById(productId);
        if (productToDelete is null)
            return NotFound();

        await _productRepository.Delete(productToDelete.ProductId);
        return NoContent();
    }
}