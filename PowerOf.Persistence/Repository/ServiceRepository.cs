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
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddEntityAsync(Service service)
        {
            await _context.services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntityAsync(int id)
        {
            var service = await _context.services.FindAsync(id);
            _context.services.Remove(service);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Service>> GetAllEntityAsync()
        {
            return await _context.services.ToListAsync();
        }

        public async Task<Service?> GetEntityByIdAsync(int id)
        {
            return await _context.services.FindAsync(id);
        }

        public async Task<List<Service>> SearchServicesAsync(string name, decimal? price)
        {
            var query = _context.services.AsQueryable(); 

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase));
            }

            if (price.HasValue)
            {
                query = query.Where(s => s.Price <= price.Value);
            }

            return await query.ToListAsync();
        }

        public async Task UpdateEntityAsync(Service service)
        {
            _context.services.Update(service);
            await _context.SaveChangesAsync();
        }
    }

}
