using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Application.Requests.UserRequests.GetUsers;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListings;

namespace AuctionNetworkBackend.Application.Repositories
{
    public interface IListingRepository
    {

        Task Create(Listing listing);
        Task<Listing?> GetListingById(long listingId);
        Task<List<Listing>> GetListingsByCategoryId(long categoryId);
        Task<List<Listing>> GetListingsBySellerId(long sellerId);
        Task Update(Listing listing);
        Task Delete(Listing listing);
        Task<ListingStatus?> GetListingStatusById(long id);
        Task<List<ListingReview>> GetReviewsByListingId(long listingId);
        Task<IEnumerable<Listing>> GetListingsWithStatus(long statusId);
        Task<List<Listing>> GetListingsByStatusAndUserId(string statusName, long userId);
        Task<List<Listing>> GetUserPurchases(long userId);
        Task<List<Listing>> GetLoggedUserBids(long userId);
        Task<List<Listing>> GetListings(GetListingsRequest request);
        Task SaveChangesAsync();
        Task CheckAndCloseAuctionsAsync();
    }
}
