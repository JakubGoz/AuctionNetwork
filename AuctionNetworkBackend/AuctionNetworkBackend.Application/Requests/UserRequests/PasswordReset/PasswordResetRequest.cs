using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.PasswordReset
{
    public class PasswordResetRequest : IRequest
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
    }
}
