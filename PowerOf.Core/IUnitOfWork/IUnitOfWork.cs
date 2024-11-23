using PowerOf.Core.Entities;
using PowerOf.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Core.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository _ProductRepository { get; }
        IServiceRepository _ServiceRepository { get; }
        IOrderRepository _orderRepository { get; }
        IInvoiceRepository _invoiceRepository { get; }

        Task<int> CompleteAsync();

    }
}
