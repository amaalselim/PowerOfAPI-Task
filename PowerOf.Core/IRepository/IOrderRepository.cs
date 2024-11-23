using PowerOf.Application.DTO_s;
using PowerOf.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Core.IRepository
{
    public interface IOrderRepository
    {
        Task AddToOrderAsync(string UserId, OrderItemDTO OrderItemDTO);
        Task<OrderDTO> GetOrderAsync(string UserId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task CancelOrderAsync(int orderId);
        Task<bool> ApproveOrderAsync(int orderId);

    }
}
