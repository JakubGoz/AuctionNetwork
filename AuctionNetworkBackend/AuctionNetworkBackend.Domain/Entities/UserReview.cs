using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Domain.Entities
{
    public class UserReview
    {
        public long Id { get; set; }
        public long ReviewerId { get; set; } 
        public User Reviewer { get; set; } 
        public long ReviewedUserId { get; set; } 
        public User ReviewedUser { get; set; }
        public bool IsLike { get; set; } 

        
        
    }
}
