using API.Controllers;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfProducts()
        {
            // Arrange
            var mockProducts = new List<ProductDto>
        {
            new ProductDto { Id = 1, ProductName = "Product 1" },
            new ProductDto { Id = 2, ProductName = "Product 2" }
        };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(2, ((List<ProductDto>)returnProducts).Count);
        }

        [Fact]
        public async Task GetById_ProductExists_ReturnsOk_WithProduct()
        {
            // Arrange
            var productId = 1;
            var product = new ProductDto { Id = productId, ProductName = "Test Product" };
            _mockService.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(productId, returnProduct.Id);
        }

        [Fact]
        public async Task GetById_ProductDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var productId = 999;
            _mockService.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync((ProductDto?)null);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new CreateProductDto { ProductName = "New Product" };
            var createdProduct = new ProductDto { Id = 1, ProductName = "New Product" };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtResult.ActionName);
            var returnProduct = Assert.IsType<ProductDto>(createdAtResult.Value);
            Assert.Equal(createdProduct.Id, returnProduct.Id);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            // Arrange
            int productId = 1;
            var dto = new UpdateProductDto { ProductName = "Updated ProductName" };

            _mockService.Setup(s => s.UpdateAsync(productId, dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(productId, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_Failure_ReturnsNotFound()
        {
            // Arrange
            int productId = 1;
            var dto = new UpdateProductDto { ProductName = "Updated ProductName" };

            _mockService.Setup(s => s.UpdateAsync(productId, dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(productId, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Success_ReturnsNoContent()
        {
            // Arrange
            int productId = 1;
            _mockService.Setup(s => s.DeleteAsync(productId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_Failure_ReturnsNotFound()
        {
            // Arrange
            int productId = 1;
            _mockService.Setup(s => s.DeleteAsync(productId)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

}
