using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class Photo
    {
        public long Id { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }

        public long? ListingId { get; set; }
        public Listing? Listing { get; set; }
    }
}
