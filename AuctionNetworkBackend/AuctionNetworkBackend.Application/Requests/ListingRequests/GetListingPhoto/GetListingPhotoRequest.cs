using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingPhoto
{
    public class GetListingPhotoRequest : IRequest<GetListingPhotoDto>
    {
        public long ListingId { get; set; }
    }
}
