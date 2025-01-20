using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class ListingReview
    {
        public long Id { get; set; }
        public long ListingId { get; set; }
        public Listing Listing { get; set; }
        public long ReviewerId { get; set; }
        public User Reviewer { get; set; }
        public int Rating { get; set; }
        public string? Description { get; set; } 



    }
}
