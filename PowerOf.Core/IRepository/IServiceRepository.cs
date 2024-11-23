using PowerOf.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Core.IRepository
{
     public interface IServiceRepository : IGenericRepository<Service>
     {
        Task<List<Service>> SearchServicesAsync(string name, decimal? price);
     }
}
