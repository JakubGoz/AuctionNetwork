using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetReviews
{
    public class GetReviewsRequest : IRequest<List<GetReviewsDto>>
    {
        public long ListingId { get; set; }
    }
}
