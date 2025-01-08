using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Pagination;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.GetUsers
{
    public class GetUsersRequest : IRequest<PagedResult<GetUsersDto>>
    {
        public string? UserName { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public int PageNumber { get; set; } = 1;
    }
}
