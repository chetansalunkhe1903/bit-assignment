using API.Controllers;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests
{
    public class ItemControllerTests
    {
        private readonly Mock<IItemService> _mockService;
        private readonly ItemController _controller;

        public ItemControllerTests()
        {
            _mockService = new Mock<IItemService>();
            _controller = new ItemController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfItems()
        {
            // Arrange
            var mockItems = new List<ItemDto>
            {
                new ItemDto { Id = 1,ItemName = "Item1", ProductId = 2, ProductName ="PR1", Quantity = 5},
                new ItemDto { Id = 2,ItemName = "Item2", ProductId = 3, ProductName ="PR3", Quantity = 4}
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockItems);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnItems = Assert.IsAssignableFrom<IEnumerable<ItemDto>>(okResult.Value);
            Assert.Equal(2, ((List<ItemDto>)returnItems).Count);
        }

        [Fact]
        public async Task GetById_ItemExists_ReturnsOk_WithItem()
        {
            // Arrange
            var itemId = 1;
            var item = new ItemDto { Id = itemId /* populate other properties */ };
            _mockService.Setup(s => s.GetByIdAsync(itemId)).ReturnsAsync(item);

            // Act
            var result = await _controller.GetById(itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnItem = Assert.IsType<ItemDto>(okResult.Value);
            Assert.Equal(itemId, returnItem.Id);
        }

        [Fact]
        public async Task GetById_ItemDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var itemId = 999;
            _mockService.Setup(s => s.GetByIdAsync(itemId)).ReturnsAsync((ItemDto?)null);

            // Act
            var result = await _controller.GetById(itemId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new CreateItemDto { /* populate properties */ };
            var createdItem = new ItemDto { Id = 1 /* populate properties */ };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(createdItem);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtResult.ActionName);
            var returnItem = Assert.IsType<ItemDto>(createdAtResult.Value);
            Assert.Equal(createdItem.Id, returnItem.Id);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            // Arrange
            int itemId = 1;
            var dto = new UpdateItemDto { /* populate properties */ };

            _mockService.Setup(s => s.UpdateAsync(itemId, dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(itemId, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_Failure_ReturnsNotFound()
        {
            // Arrange
            int itemId = 1;
            var dto = new UpdateItemDto { /* populate properties */ };

            _mockService.Setup(s => s.UpdateAsync(itemId, dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(itemId, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Success_ReturnsNoContent()
        {
            // Arrange
            int itemId = 1;
            _mockService.Setup(s => s.DeleteAsync(itemId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(itemId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_Failure_ReturnsNotFound()
        {
            // Arrange
            int itemId = 1;
            _mockService.Setup(s => s.DeleteAsync(itemId)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(itemId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
