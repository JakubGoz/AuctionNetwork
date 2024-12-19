using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Infrastructure.EF.Options
{
    public class AuthenticationSettings
    {
        /// <summary>
        /// Jwt Key.
        /// </summary>
        public string JwtKey { get; set; }

        /// <summary>
        /// Days till Jwt token expires.
        /// </summary>
        public int JwtExpireDays { get; set; }

        /// <summary>
        /// Jwt issuer.
        /// </summary>
        public string JwtIssuer { get; set; }
    }
}
