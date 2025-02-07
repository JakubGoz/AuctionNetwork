using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.UpdateListing
{
    public class UpdateListingRequest : IRequest
    {
        public long ListingId { get;  }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? BuyNowPrice { get; set; }
        public DateTime? EndDate { get; set; }
        public long CategoryId { get; set; }
        public bool IsAuction { get; set; }
        public IFormFile? ListingPicture { get; set; }

        public UpdateListingRequest(long listingId) // Konstruktor wymaga listingId
        {
            ListingId = listingId;
        }
    }
}
