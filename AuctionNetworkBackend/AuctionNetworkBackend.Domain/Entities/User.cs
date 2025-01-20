using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }

        public required string Email { get; set; }

        public required string UserName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public Role Role { get; set; }

        public long RoleId { get; set; }
        public string PasswordHash { get; set; }


        public List<Listing> Listings { get; set; }
        public List<Bid> Bids { get; set; } // Lista ofert złożonych przez użytkownika
        public List<UserReview> UserReviews { get; set; } // Lista recenzji użytkownika
    }
}
