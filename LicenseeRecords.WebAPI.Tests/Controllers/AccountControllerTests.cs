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
    public class AccountControllerTests
    {
        private Mock<IAccountRepository> _mockAccountRepository;
        private AccountController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _controller = new AccountController(_mockAccountRepository.Object);
        }

        [TestMethod]
        public async Task GetAccounts()
        {
            // Arrange
            var mockAccounts = new List<Account>
            {
                new Account { AccountId = 1, AccountName = "Account1", AccountStatus = Status.Active  },
                new Account { AccountId = 2, AccountName = "Account2",  AccountStatus = Status.Active }
            };

            _mockAccountRepository.Setup(repo => repo.GetAllAccounts()).ReturnsAsync(mockAccounts);

            // Act
            var result = await _controller.GetAccounts();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var accounts = okResult.Value as IEnumerable<Account>;
            Assert.IsNotNull(accounts);
            Assert.AreEqual(2, ((List<Account>)accounts).Count);
        }

        // [TestMethod]
        // public async Task GetProduct()
        // {
        //     // Arrange
        //     var productId = 1;
        //     var mockProduct = new Product { ProductId = productId, ProductName = "Product1" };
        //     _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(mockProduct);

        //     // Act
        //     var result = await _controller.GetProduct(productId);

        //     // Assert
        //     var okResult = result as OkObjectResult;
        //     Assert.IsNotNull(okResult);
        //     var product = okResult.Value as Product;
        //     Assert.IsNotNull(product);
        //     Assert.AreEqual(productId, product.ProductId);
        // }

        // [TestMethod]
        // public async Task GetProductById()
        // {
        //     // Arrange
        //     var productId = 999;
        //     _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync((Product)null);

        //     // Act
        //     var result = await _controller.GetProduct(productId);

        //     // Assert
        //     var notFoundResult = result as NotFoundResult;
        //     Assert.IsNotNull(notFoundResult);
        // }

        // [TestMethod]
        // public async Task UpdateProduct()
        // {
        //     // Arrange
        //     var productId = 1;
        //     var updatedProduct = new Product { ProductId = productId, ProductName = "Updated Product" };
        //     _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(updatedProduct);
        //     _mockProductRepository.Setup(repo => repo.GetProductByName(It.IsAny<string>())).ReturnsAsync((Product)null);
        //     _mockProductRepository.Setup(repo => repo.Update(It.IsAny<Product>())).Returns(Task.CompletedTask);

        //     // Act
        //     var result = await _controller.Update(productId, updatedProduct);

        //     // Assert
        //     var noContentResult = result as NoContentResult;
        //     Assert.IsNotNull(noContentResult);
        // }

        // [TestMethod]
        // public async Task DeleteProduct()
        // {
        //     // Arrange
        //     var productId = 1;
        //     var mockProduct = new Product { ProductId = productId, ProductName = "Product1" };
        //     _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(mockProduct);
        //     _mockProductRepository.Setup(repo => repo.Delete(productId)).Returns(Task.CompletedTask);

        //     // Act
        //     var result = await _controller.Delete(productId);

        //     // Assert
        //     var noContentResult = result as NoContentResult;
        //     Assert.IsNotNull(noContentResult);
        // }
    }
}
