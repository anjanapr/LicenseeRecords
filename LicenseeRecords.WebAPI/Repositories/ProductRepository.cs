using System.Text.Json;
using LicenseeRecords.WebAPI.Interfaces;
using LicenseeRecords.WebAPI.Models;

namespace LicenseeRecords.WebAPI.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly string _productsFilePath;
    public ProductRepository(string filePath)
    {
        _productsFilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    private async Task<List<Product>> ReadFromFile()
    {
        try
        {
            if (!File.Exists(_productsFilePath))
            {
                return new List<Product>();
            }
            var json = await File.ReadAllTextAsync(_productsFilePath);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading from file", ex);
        }
    }

    private async Task SaveToFile(List<Product> products)
    {
        try
        {
            var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_productsFilePath, json);
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving to file", ex);
        }
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await ReadFromFile();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return (await ReadFromFile()).FirstOrDefault(item => item.ProductId == id);
    }

    public async Task<Product?> GetProductByName(string name)
    {
        return (await ReadFromFile())
            .Where(item => item.ProductName.Trim().ToLower() == name.Trim().ToLower())
            .FirstOrDefault();
    }

    public async Task<Product> Create(Product product)
    {
        var products = await ReadFromFile();
        product.ProductId = products.Any() ? products.Max(item => item.ProductId) + 1 : 1;
        products.Add(product);
        await SaveToFile(products);
        return product;
    }

    public async Task Update(Product product)
    {
        var products = await ReadFromFile();
        var existingProduct = products.FirstOrDefault(item => item.ProductId == product.ProductId);
        if (existingProduct == null)
            throw new KeyNotFoundException($"Product with id {product.ProductId} not found");

        existingProduct.ProductName = product.ProductName;
        await SaveToFile(products);
    }

    public async Task Delete(int id)
    {
        var products = await ReadFromFile();
        var productToDelete = products.FirstOrDefault(item => item.ProductId == id);
        if (productToDelete == null)
            throw new KeyNotFoundException($"Product with id {id} not found");

        products.Remove(productToDelete);
        await SaveToFile(products);
    }
}