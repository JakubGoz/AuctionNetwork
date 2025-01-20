using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace AuctionNetworkBackend.Application.Requests.ListingRequests.AddListingReview
{
    public class AddListingReviewRequest : IRequest
    {
        public long ListingId { get; set; }
        public int Rating { get; set; } // Wartość od 1 do 5
        public string? Description { get; set; } // Opcjonalny opis
    }
}
