using LicenseeRecords.WebAPI.Models;

namespace LicenseeRecords.WebAPI.Interfaces;
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product?> GetProductById(int id);
    Task<Product?> GetProductByName(string name);
    Task<Product> Create(Product product);
    Task Update(Product product);
    Task Delete(int id);
}