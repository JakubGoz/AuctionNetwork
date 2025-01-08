using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.GetUsers
{
    public class GetUsersDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
