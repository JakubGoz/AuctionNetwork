using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.BidRequests.GetListingBids
{
    public class GetListingBidsRequest : IRequest<List<GetListingBidsDto>>
    {
        public long ListingId { get; set; }
    }
}
