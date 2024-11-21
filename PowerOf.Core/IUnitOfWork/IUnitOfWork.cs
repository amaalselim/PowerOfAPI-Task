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
        IGenericRepository<Product> _ProductRepository { get; }
        Task<int> Complete();

    }
}
