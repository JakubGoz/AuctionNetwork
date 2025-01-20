using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Domain.Entities;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingDetailsById
{
    public class GetListingDetailsByIdDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? BuyNowPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ListingStatus { get; set; }
        public bool IsAuction { get; set; }
        public bool IsItMyListing { get; set; }
        public long SellerId { get; set; }
        public string SellerUserName { get; set; }
        public string SellerEmail { get; set; }
        public string? SellerPhoneNumber { get; set; }
        public string CategoryName { get; set; }
        public List<Bid>? Bids { get; set; }
        public int ListingReviewsCount { get; set; }
        public int SellerReviewsCount { get; set; }

    }
}
