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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AuctionNetworkDbContext _dbContext;
        public PaymentRepository (AuctionNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Payment> AddPayment(Payment payment)
        {
            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();
            return payment; // Zwracamy nowo utworzoną płatność
        }

        public async Task<Payment> GetPaymentById(long paymentId)
        {
            return await _dbContext.Payments
                .Include(p => p.Order)      // Zakładając, że Payment ma relację z Order
                .ThenInclude(o => o.Listing) // Zakładając, że Order ma relację z Listing
                .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
