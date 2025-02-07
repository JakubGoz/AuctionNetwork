using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.CreateListing
{
    public class CreateListingRequest: IRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? BuyNowPrice { get; set; }
        public DateTime? EndDate { get; set; }
        public long CategoryId { get; set; }
        public bool IsAuction { get; set; }
        public IFormFile? ListingPicture { get; set; }

    }
}
