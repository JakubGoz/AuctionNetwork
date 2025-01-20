using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Application.Pagination;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingsByCategory
{
    public class GetListingsByCategoryRequest : IRequest<PagedResult<GetListingsByCategoryDto>>
    {
        public long CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
    }
}
