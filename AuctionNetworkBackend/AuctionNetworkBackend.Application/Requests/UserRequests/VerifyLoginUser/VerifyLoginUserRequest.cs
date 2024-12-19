using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.VerifyLoginUser
{
    public class VerifyLoginUserRequest : IRequest<VerifyLoginUserDto>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Token { get; set; }
    }
}
