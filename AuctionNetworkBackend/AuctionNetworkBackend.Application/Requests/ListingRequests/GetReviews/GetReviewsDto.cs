using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetReviews
{
    public class GetReviewsDto
    {
        public long ReviewerId { get; set; }
        public string ReviewerUserName { get; set; }
        public int Rating { get; set; }
        public string? Description { get; set; }
    }
}
