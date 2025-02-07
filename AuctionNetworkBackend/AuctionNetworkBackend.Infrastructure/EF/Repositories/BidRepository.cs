using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Infrastructure.EF.Contexts;
using Microsoft.Extensions.Hosting;


namespace AuctionNetworkBackend.Infrastructure.EF.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AuctionNetworkDbContext _dbContext;
        public BidRepository(AuctionNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Bid bid)
        {
            await _dbContext.Bids.AddAsync(bid);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Bid?> GetBidByUserIdAndListingId(long userId, long listingId)
        {
            return await _dbContext.Bids
                .FirstOrDefaultAsync(b => b.BuyerId == userId && b.ListingId == listingId);
        }

        public async Task Update(Bid bid)
        {
            _dbContext.Bids.Update(bid);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Bid>> GetBidsByListingId(long listingId)
        {
            return await _dbContext.Bids
                .Include(b => b.Buyer)
                .Where(b => b.ListingId == listingId)
                .ToListAsync();
                 
        }
    }
}
