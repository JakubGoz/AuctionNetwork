using AuctionNetworkBackend.Application.Pagination;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetLoggedUserPurchases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.BidRequests.GetLoggedUserBids
{
    public class GetLoggedUserBidsRequest: IRequest<PagedResult<GetLoggedUserBidsDto>>
    {
        public int PageNumber { get; set; } = 1;
    }
}
