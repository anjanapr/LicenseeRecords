using LicenseeRecords.WebAPI.Controllers;
using LicenseeRecords.WebAPI.Interfaces;
using LicenseeRecords.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LicenseeRecords.WebAPI.Tests
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _controller = new ProductController(_mockProductRepository.Object);
        }

        [TestMethod]
        public async Task GetProducts()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Product1" },
                new Product { ProductId = 2, ProductName = "Product2" }
            };

            _mockProductRepository.Setup(repo => repo.GetAllProducts()).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var products = okResult.Value as IEnumerable<Product>;
            Assert.IsNotNull(products);
            Assert.AreEqual(2, ((List<Product>)products).Count);
        }

        [TestMethod]
        public async Task GetProduct()
        {
            // Arrange
            var productId = 1;
            var mockProduct = new Product { ProductId = productId, ProductName = "Product1" };
            _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(mockProduct);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var product = okResult.Value as Product;
            Assert.IsNotNull(product);
            Assert.AreEqual(productId, product.ProductId);
        }

        [TestMethod]
        public async Task GetProductById()
        {
            // Arrange
            var productId = 999;
            _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }

        [TestMethod]
        public async Task UpdateProduct()
        {
            // Arrange
            var productId = 1;
            var updatedProduct = new Product { ProductId = productId, ProductName = "Updated Product" };
            _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(updatedProduct);
            _mockProductRepository.Setup(repo => repo.GetProductByName(It.IsAny<string>())).ReturnsAsync((Product)null);
            _mockProductRepository.Setup(repo => repo.Update(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(productId, updatedProduct);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
        }

        [TestMethod]
        public async Task DeleteProduct()
        {
            // Arrange
            var productId = 1;
            var mockProduct = new Product { ProductId = productId, ProductName = "Product1" };
            _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(mockProduct);
            _mockProductRepository.Setup(repo => repo.Delete(productId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
        }
    }
}
