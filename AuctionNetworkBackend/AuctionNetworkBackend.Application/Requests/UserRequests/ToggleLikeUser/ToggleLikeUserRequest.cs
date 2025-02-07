using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace AuctionNetworkBackend.Application.Requests.UserRequests.ToggleLikeUser
{
    public class ToggleLikeUserRequest : IRequest
    {
        public long UserId { get; set; }
        public bool ThumbUp { get; set; }
    }
}
