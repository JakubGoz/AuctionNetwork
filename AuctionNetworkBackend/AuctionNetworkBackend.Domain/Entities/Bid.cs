using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class Bid
    {
        public long Id { get; set; } // Id oferty
        public long ListingId { get; set; } // Powiązanie z listingiem
        public Listing Listing { get; set; }


        public long BuyerId { get; set; } // Id kupującego
        public User Buyer { get; set; } // Kupujący (odwołanie do użytkownika)
        public decimal Price { get; set; } // Kwota oferty
        public DateTime Date { get; set; } // Data oferty
    }
}
