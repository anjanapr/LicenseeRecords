using LicenseeRecords.Web.Interfaces;
using LicenseeRecords.Web.Models;

namespace LicenseeRecords.Web.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProducts()
    {
        try
        {
            var result = await _httpClient.GetAsync("product");
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to load products", ex);
        }
    }

    public async Task<Product?> GetProductById(int productId)
    {
        try
        {
            var result = await _httpClient.GetAsync($"product/{productId}");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Product>();
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to retrieve Product with id {productId}", ex);
        }
    }

    public async Task Post(Product product)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync("product", product);
            result.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create product", ex);
        }
    }

    public async Task Put(Product product)
    {
        try
        {
            var result = await _httpClient.PutAsJsonAsync($"product/{product.ProductId}", product);
            result.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update product", ex);
        }
    }

    public async Task Delete(int productId)
    {
        try
        {
            var result = await _httpClient.DeleteAsync($"product/{productId}");
            result.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete product", ex);
        }
    }
}