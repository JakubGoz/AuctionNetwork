using AuctionNetworkBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order order);
        Task<Order> GetOrderById(long orderId);
        Task<List<Order>> GetOrdersByUserId(long userId);
        Task UpdateOrder(Order order);
        Task DeleteOrder(long orderId);
        Task SaveChangesAsync();
    }
}
