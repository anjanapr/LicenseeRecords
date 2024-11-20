using LicenseeRecords.Web.Models;

namespace LicenseeRecords.Web.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProducts();
    Task<Product> GetProductById(int productId);
    Task Post(Product product);
    Task Put(Product product);
    Task Delete(int productId);
}