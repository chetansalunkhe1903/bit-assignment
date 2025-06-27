using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items
            .Include(i => i.Product)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items
            .Include(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Item> AddAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item item)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Item?> GetWithProductAsync(int id)
        {
            return await _context.Items
            .Include(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
