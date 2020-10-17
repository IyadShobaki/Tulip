using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tulip_API.Contracts;
using Tulip_API.Data;

namespace Tulip_API.Services
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _db;

        public InventoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Inventory entity)
        {
            await _db.Inventories.AddAsync(entity);
            return await Save();

        }

        public async Task<bool> Delete(Inventory entity)
        {
            _db.Inventories.Remove(entity);
            return await Save();
        }

        public async Task<IList<Inventory>> FindAll()
        {
            var inventories = await _db.Inventories.ToListAsync();
            return inventories;
        }

        public async Task<Inventory> FindById(int id)
        {
            var inventory = await _db.Inventories.FindAsync(id);
            return inventory;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Update(Inventory entity)
        {
            _db.Inventories.Update(entity);
            return await Save();
        }
    }
}
