using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.LoginUser
{
    public class LoginUserRequest : IRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
