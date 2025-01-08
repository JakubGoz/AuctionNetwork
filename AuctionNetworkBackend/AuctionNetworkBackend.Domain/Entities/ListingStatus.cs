using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public enum ListingStatus
    {
        Active,  // Ogłoszenie jest aktywne
        Sold,    // Przedmiot został sprzedany
        Ended    // Aukcja zakończona
    }
}
