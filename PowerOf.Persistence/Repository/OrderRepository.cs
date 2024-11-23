using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PowerOf.Application.DTO_s;
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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
       

        

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            var orders = await _context.orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();
            return orders;
        }

        public async Task<bool> ApproveOrderAsync(int orderId)
        {
            var order = await _context.orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order != null && order.Status == "Pending")
            {
                order.Status = "Approved";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
       

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _context.orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
        }


        public async Task AddToOrderAsync(string UserId, OrderItemDTO OrderItemDTO)
        {
            if (OrderItemDTO.Quantity == 0)
            {
               OrderItemDTO.Quantity = 1;
            }

            var OrderDTO = await GetOrderAsync(UserId);
            Order order;

            if (OrderDTO == null)
            {
                order = new Order { UserId = UserId };
                await _context.orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                order = await _context.orders.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == UserId);
            }

            var existingItem = order.Items.FirstOrDefault(i => i.ProductId == OrderItemDTO.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity = OrderItemDTO.Quantity;
                existingItem.Price = existingItem.Product.Price;
            }
            else
            {
                var orderItem = _mapper.Map<OrderItem>(OrderItemDTO);
               orderItem.OrderId = order.Id;

                var product = await _context.Products.FindAsync(OrderItemDTO.ProductId);
                if (product != null)
                {
                    orderItem.Product = product;
                    orderItem.Price = product.Price;
                }

                order.Items.Add(orderItem);
            }

            order.TotalPrice = order.Items.Sum(item => item.Quantity * item.Price);

            _context.orders.Update(order);
            await _context.SaveChangesAsync();
        }


       
        public async Task<OrderDTO> GetOrderAsync(string UserId)
        {
            var order= await _context.orders
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (order == null || !order.Items.Any())
            {
                return null;
            }
            return _mapper.Map<OrderDTO>(order);
        }


    }
}
