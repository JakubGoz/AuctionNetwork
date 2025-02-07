using AuctionNetworkBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingsByCategory
{
    public class GetListingsByCategoryDto
    {
        public long SellerId { get; set; }
        public string SellerUserName { get; set; }
        public long ListingId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal? BuyNowPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ListingStatus { get; set; }
        public bool IsAuction { get; set; }
        public int ListingReviewsCount { get; set; }
        public double ListingReviewsAvg { get; set; }

        public bool IsLiked { get; set; }


    }
}
