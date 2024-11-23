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
        

        public IProductRepository _ProductRepository {  get; private set; }

        public IServiceRepository _ServiceRepository { get; private set; }

        public IOrderRepository _orderRepository { get; private set; }

        public IInvoiceRepository _invoiceRepository {  get; private set; }

        public UnitOfWork(
            ApplicationDbContext context,
            IProductRepository product,
            IServiceRepository service,
            IOrderRepository order,
            IInvoiceRepository invoice
            )
        {
            _context = context;
            _ProductRepository = product;
            _ServiceRepository = service;
            _orderRepository = order;
            _invoiceRepository = invoice;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
