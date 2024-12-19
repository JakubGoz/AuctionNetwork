using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace AuctionNetworkBackend.Application.Requests.UserRequests.VerifyRegisterUser
{
    public class VerifyRegisterUserRequest : IRequest
    {
        public string Email { get; set; }
    }
}
