using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Core.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddEntityAsync(T entity);
        Task<IEnumerable<T>> GetAllEntityAsync();
        Task<T?> GetEntityByIdAsync(string id);
        Task UpdateEntityAsync(T entity);
        Task DeleteEntityAsync(string id);
    }
}
