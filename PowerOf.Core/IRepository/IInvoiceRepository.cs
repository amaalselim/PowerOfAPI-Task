using PowerOf.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Core.IRepository
{
    public interface IInvoiceRepository
    {
        Task<Invoice> CreateInvoiceAsync(int orderId);
        Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(string userId);
        Task<Invoice> GetInvoiceByIdAsync(int invoiceId);
    }
}
