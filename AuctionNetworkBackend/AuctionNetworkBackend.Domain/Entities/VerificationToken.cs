using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class VerificationToken
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string TokenHash { get; set; }

        public DateTime Created { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
