using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tulip_API.Contracts;
using Tulip_API.Data;

namespace Tulip_API.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Category entity)
        {
            await _db.Categories.AddAsync(entity);
            return await Save();

        }

        public async Task<bool> Delete(Category entity)
        {
            _db.Categories.Remove(entity);
            return await Save();
        }

        public async Task<IList<Category>> FindAll()
        {
            var inventories = await _db.Categories
                .Include(p => p.Products)
                .ToListAsync();
            return inventories;
        }

        public async Task<Category> FindById(int id)
        {
            var inventory = await _db.Categories
                .Include(p => p.Products)
                //.FindAsync(id);
                .FirstOrDefaultAsync(p => p.Id == id);
            return inventory;
        }

        public async Task<bool> IsExists(int id)
        {
            return await _db.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Update(Category entity)
        {
            _db.Categories.Update(entity);
            return await Save();
        }
    }
}
