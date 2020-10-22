using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tulip_API.Contracts;
using Tulip_API.Data;

namespace Tulip_API.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Product entity)
        {
            await _db.Products.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Product entity)
        {
            _db.Products.Remove(entity);
            return await Save();
        }

        public async Task<IList<Product>> FindAll()
        {
            var product = await _db.Products
                .Include(q => q.Category)
                .ToListAsync();
            return product;
        }

        public async Task<Product> FindById(int id)
        {
            var product = await _db.Products//.FindAsync(id);
                .Include(q => q.Category)
                .FirstOrDefaultAsync(q => q.Id == id);
            return product;
        }

        public async Task<bool> IsExists(int id)
        {
            var isExists = await _db.Products.AnyAsync(p => p.Id == id);
            return isExists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Product entity)
        {
            _db.Products.Update(entity);         
            return await Save();
        }
    }
}
