using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Application.Pagination;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetUsersListings
{
    public class GetUsersListingsRequest : IRequest<PagedResult<GetUsersListingsDto>>
    {
        public long UserId { get; set; }
        public int PageNumber { get; set; } = 1;

    }
}
