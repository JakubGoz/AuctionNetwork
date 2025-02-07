using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Application.Pagination;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingsByCategory;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListings
{
    public class GetListingsRequest : IRequest<PagedResult<GetListingsDto>>
    {
        public string? SearchQuery { get; set; }
        public long? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAuction { get; set; }
        public int? OrderBy { get; set; }

        public int PageNumber { get; set; } = 1;

    }
}
