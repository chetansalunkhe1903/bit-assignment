using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return null;
            var productDto = _mapper.Map<ProductDto>(product);

            // Load related items
            var relatedItems = await _productRepository.GetRelatedItemsAsync(product.Id);
            productDto.RelatedItems = _mapper.Map<List<RelatedItemDto>>(relatedItems);

            return productDto;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createDto)
        {
            var product = _mapper.Map<Product>(createDto);
            product.CreatedOn = DateTime.UtcNow;
            product.CreatedBy = "system"; // Replace with current user if available

            await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            product.ProductName = updateDto.ProductName;
            product.ModifiedOn = DateTime.UtcNow;
            product.ModifiedBy = "system"; // Replace with current user

            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }
    }
}
