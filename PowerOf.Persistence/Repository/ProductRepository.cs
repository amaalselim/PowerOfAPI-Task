using Microsoft.EntityFrameworkCore;
using PowerOf.Core.Entities;
using PowerOf.Core.IRepository;
using PowerOf.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Persistence.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
           _context = context;
        }
        public async Task AddEntityAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntityAsync(int id)
        {
            var product= await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllEntityAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetEntityByIdAsync(int id)
        {
           return await _context.Products.FindAsync(id);
        }

        public async Task UpdateEntityAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Product>> SearchProductAsync(string searchTerm)
        {
            var lowersearchterm = searchTerm.ToLower();
            return await _context.Products
                .Where(p => p.Name.ToLower().StartsWith(lowersearchterm)).ToListAsync();
        }
    }
}
