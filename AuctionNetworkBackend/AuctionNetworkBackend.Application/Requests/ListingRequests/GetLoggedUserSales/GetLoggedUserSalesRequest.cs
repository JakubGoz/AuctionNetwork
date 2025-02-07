using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Application.Pagination;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetLoggedUserSales
{
    public class GetLoggedUserSalesRequest : IRequest<PagedResult<GetLoggedUserSalesDto>>
    {
        public int PageNumber { get; set; } = 1;

    }
}
