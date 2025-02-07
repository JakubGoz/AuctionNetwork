using AuctionNetworkBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Repositories
{
    public interface IBidRepository
    {
        Task Create(Bid bid);
        Task<Bid?> GetBidByUserIdAndListingId(long userId, long listingId);
        Task Update(Bid bid);
        Task<List<Bid>> GetBidsByListingId(long listingId);

    }
}
