using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; } // Nazwa kategorii
        public List<Listing> Listings { get; set; }
    }
}
