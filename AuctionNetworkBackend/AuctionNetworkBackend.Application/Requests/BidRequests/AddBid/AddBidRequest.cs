using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuctionNetworkBackend.Application.Requests.BidRequests.AddBid
{
    public class AddBidRequest : IRequest
    {
        public long ListingId { get; set; }
        public decimal Price { get; set; }
    }
}
