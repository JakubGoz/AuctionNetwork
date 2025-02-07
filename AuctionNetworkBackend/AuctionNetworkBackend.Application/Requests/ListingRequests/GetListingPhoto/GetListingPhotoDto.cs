using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingPhoto
{
    public class GetListingPhotoDto
    {
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
    }
}
