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
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Invoice> CreateInvoiceAsync(int orderId)
        {
            var order = await _context.orders.Where(i=>i.Status=="Pending")
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new ArgumentException("Order not found.");
            }

            var tax = order.TotalPrice * 0.05m; //5% tax
            var totalAmount = order.TotalPrice + tax;

            var invoice = new Invoice
            {
                OrderId = orderId,
                InvoiceNumber = $"INV-{DateTime.Now:yyyyMMddHHmmss}-{orderId}",
                TotalAmount = totalAmount,
                Tax = tax,
                
            };

            await _context.invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(string userId)
        {
            return await _context.invoices
                .Include(i => i.Order)
                .Where(i => i.Order.UserId == userId)
                .ToListAsync();
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _context.invoices
                .Include(i => i.Order)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);
        }
    }
}
