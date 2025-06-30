using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
   [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Get all Products")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                _logger.LogInformation("Retrieved {Count} products.", products.Count());
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all products.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [SwaggerOperation(Summary = "Get Product details and related Item details by Product ID")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with Id = {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Retrieved product details for Id = {Id}.", id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product with Id = {Id}.", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [SwaggerOperation(Summary = "Add a new Product")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            try
            {
                var product = await _productService.CreateAsync(dto);
                _logger.LogInformation("Product created successfully with Id = {Id}.", product.Id);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new product.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [SwaggerOperation(Summary = "Update the Product")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            try
            {
                var success = await _productService.UpdateAsync(id, dto);
                if (!success)
                {
                    _logger.LogWarning("Product update failed. Product with Id = {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Product with Id = {Id} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with Id = {Id}.", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [SwaggerOperation(Summary = "Delete a Product")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _productService.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Product delete failed. Product with Id = {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Product with Id = {Id} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with Id = {Id}.", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
