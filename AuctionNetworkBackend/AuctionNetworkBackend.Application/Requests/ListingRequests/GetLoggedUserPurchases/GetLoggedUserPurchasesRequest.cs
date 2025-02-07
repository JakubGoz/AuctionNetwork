using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Application.Pagination;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetLoggedUserPurchases
{
    public class GetLoggedUserPurchasesRequest : IRequest<PagedResult<GetLoggedUserPurchasesDto>>
    {
        public int PageNumber { get; set; } = 1;

    }
}
