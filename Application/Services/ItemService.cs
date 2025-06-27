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
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<ItemDto?> GetByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            return _mapper.Map<ItemDto?>(item);
        }

        public async Task<ItemDto> CreateAsync(CreateItemDto dto)
        {
            var item = _mapper.Map<Item>(dto);
            item = await _itemRepository.AddAsync(item);

            // Refetch with Product included
            var createdItem = await _itemRepository.GetWithProductAsync(item.Id);
            return _mapper.Map<ItemDto>(createdItem);
        }

        public async Task<bool> UpdateAsync(int id, UpdateItemDto dto)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem == null)
                return false;

            _mapper.Map(dto, existingItem);
            await _itemRepository.UpdateAsync(existingItem);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem == null)
                return false;

            await _itemRepository.DeleteAsync(existingItem);
            return true;
        }
    }
}
