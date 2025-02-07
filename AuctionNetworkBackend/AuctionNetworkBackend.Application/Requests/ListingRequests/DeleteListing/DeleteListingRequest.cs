using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.DeleteListing
{
    public class DeleteListingRequest:IRequest
    {
        public long ListingId { get; set; }

    }
}
