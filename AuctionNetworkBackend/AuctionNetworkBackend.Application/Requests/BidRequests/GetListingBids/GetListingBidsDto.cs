using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.BidRequests.GetListingBids
{
    public class GetListingBidsDto
    {
        public string UserUserName { get; set; }
        public decimal Price { get; set; }
        public DateTime BidDate { get; set; }
    }
}
