using PowerOf.Core.Entities;
using PowerOf.Core.IRepository;
using PowerOf.Core.IUnitOfWork;
using PowerOf.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<Product> _ProductRepository {  get; private set; }
        public UnitOfWork(ApplicationDbContext context,IGenericRepository<Product> product)
        {
            _context = context;
            _ProductRepository = product;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
