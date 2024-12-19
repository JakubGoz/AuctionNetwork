using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.VerifyLoginUser
{
    public class VerifyLoginUserDto
    {
        /// <summary>
        /// Jwt token used for authorization.
        /// </summary>
        public string Token { get; set; }
    }
}
