using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Infrastructure.EF.Contexts;

namespace AuctionNetworkBackend.Infrastructure.EF.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AuctionNetworkDbContext _dbContext;

        public OrderRepository(AuctionNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Dodawanie nowego zamówienia
        public async Task<Order> AddOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order; // Zwracamy nowo utworzone zamówienie
        }

        public async Task<Order?> GetOrderById(long orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.Payment) 
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetOrdersByUserId(long userId)
        {
            return await _dbContext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Payment) // Jeśli chcesz dołączyć dane o płatności
                .ToListAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }   

        // Usuwanie zamówienia (jeśli potrzebujesz)
        public async Task DeleteOrder(long orderId)
        {
            var order = await GetOrderById(orderId);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
