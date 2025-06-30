using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IItemService itemService, ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Get all Items")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _itemService.GetAllAsync();
                _logger.LogInformation("GetAll executed successfully. Retrieved {Count} items.", items.Count());
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAll while retrieving items.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [SwaggerOperation(Summary = "Get Item details by Id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _itemService.GetByIdAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("GetById did not find an item with Id = {Id}", id);
                    return NotFound();
                }

                _logger.LogInformation("GetById retrieved item successfully. Id = {Id}", id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetById for item Id = {Id}", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [SwaggerOperation(Summary = "Create a new Item")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateItemDto createItemDto)
        {
            try
            {
                var createdItem = await _itemService.CreateAsync(createItemDto);
                _logger.LogInformation("Create executed successfully. Created Item Id = {Id}", createdItem.Id);
                return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Create while creating item.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [SwaggerOperation(Summary = "Update the Item")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateItemDto dto)
        {
            try
            {
                var success = await _itemService.UpdateAsync(id, dto);
                if (!success)
                {
                    _logger.LogWarning("Update failed. Item not found for Id = {Id}", id);
                    return NotFound();
                }

                _logger.LogInformation("Update executed successfully for Item Id = {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Update for Item Id = {Id}", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [SwaggerOperation(Summary = "Delete an Item")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _itemService.DeleteAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Delete failed. Item not found for Id = {Id}", id);
                    return NotFound();
                }

                _logger.LogInformation("Delete executed successfully for Item Id = {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Delete for Item Id = {Id}", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
