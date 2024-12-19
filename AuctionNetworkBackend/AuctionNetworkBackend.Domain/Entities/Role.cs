using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class Role
    {
        public long Id { get; set; }

        public required string Name { get; set; }
    }
}
