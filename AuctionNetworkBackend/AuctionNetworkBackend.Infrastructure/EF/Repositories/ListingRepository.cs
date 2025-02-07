using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Infrastructure.EF.Contexts;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListings;

namespace AuctionNetworkBackend.Infrastructure.EF.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly AuctionNetworkDbContext _dbContext;
        public ListingRepository(AuctionNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Listing listing)
        {
            await _dbContext.Listings.AddAsync(listing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Listing>> GetListings(GetListingsRequest request)
        {
            return await _dbContext.Listings
                .Where(l => l.Status.Id == 1)
                .Include(l => l.Status)
                .Include(l => l.ListingReviews)
                .Include(l => l.Seller)
                .ToListAsync();
        }
        public async Task<Listing?> GetListingById(long listingId)
        {
            return await _dbContext.Listings
                .Include(l => l.Category)
                .Include(l => l.Status) // Załaduj Status
                .Include(l => l.ListingReviews)
                .Include(l => l.Seller)
                .FirstOrDefaultAsync(l => l.Id == listingId);
        }

        public async Task<List<Listing>> GetListingsByCategoryId(long categoryId)
        {
            return await _dbContext.Listings
                .Where(l => l.CategoryId == categoryId && l.Status.Id == 1)
                .Include(l => l.Status)
                .Include(l => l.ListingReviews)
                .Include(l => l.Seller)
                .ToListAsync();
        }

        public async Task<List<Listing>> GetListingsBySellerId(long sellerId)
        {
            return await _dbContext.Listings
                .Include(l => l.Status)
                .Include(l => l.ListingReviews)
                .Include(l => l.Seller)
                .Where(l => l.SellerId == sellerId)
                .ToListAsync();
        }

        public async Task Update(Listing listing)
        {
            _dbContext.Listings.Update(listing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Listing listing)
        {
            _dbContext.Listings.Remove(listing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ListingStatus?> GetListingStatusById(long id)
        {
            return await _dbContext.ListingsStatus
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        // Dodana metoda do pobierania recenzji dla listingów
        public async Task<List<ListingReview>> GetReviewsByListingId(long listingId)
        {
            return await _dbContext.ListingReviews
                .Where(review => review.ListingId == listingId)
                .Include(review => review.Reviewer)  // Można dodać informacje o użytkowniku, który wystawił recenzję
                .ToListAsync();
        }
        public async Task<IEnumerable<Listing>> GetListingsWithStatus(long statusId)
        {
            return await _dbContext.Listings
                .Where(l => l.Status.Id == statusId)
                .ToListAsync();
        }

        public async Task<List<Listing>> GetListingsByStatusAndUserId(string statusName, long userId)
        {
            return await _dbContext.Listings
                .Include(l => l.Status)
                .Include(l => l.ListingReviews)
                .Include(l => l.Seller)
                .Where(l => l.Status.Name == statusName && l.SellerId == userId)
                .ToListAsync();
        }

        public async Task<List<Listing>> GetUserPurchases(long userId)
        {
            return await _dbContext.Listings
                .Include(l => l.Status)
                .Include(l => l.ListingReviews)
                .Include(l => l.Seller)
                .Where(l => l.BuyerId == userId)
                .ToListAsync();
        }
        public async Task<List<Listing>> GetUserSales(long userId)
        {
            return await _dbContext.Listings
                .Include(l => l.Status)
                .Include(l => l.ListingReviews)
                .Include(l => l.Seller)
                .Where(l => l.SellerId == userId)
                .ToListAsync();
        }

        public async Task<List<Listing>> GetLoggedUserBids(long userId)
        {
            return await _dbContext.Listings
                .Include(l => l.Status)
                .Include(l => l.ListingReviews)
                .Include(l => l.Seller)
                .Where(l =>  l.Bids.Any(b => b.BuyerId == userId))
                .ToListAsync();
        }
        public async Task CheckAndCloseAuctionsAsync()
        {
            var now = DateTime.UtcNow;
            var expiredListings = await _dbContext.Listings
                .Where(l => l.EndDate <= now && l.IsAuction && l.ListingStatusId != 2)
                .Include(l => l.Bids)
                .ToListAsync();

            foreach (var listing in expiredListings)
            {
                listing.ListingStatusId = 2; 

                var highestBid = listing.Bids?.OrderByDescending(b => b.Price).FirstOrDefault();
                if (highestBid != null)
                {
                    listing.WinnerId = highestBid.BuyerId;
                }

                _dbContext.Listings.Update(listing);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
