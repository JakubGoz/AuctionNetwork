using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Domain.Entities;

namespace AuctionNetworkBackend.Application.Services
{
    public interface IJwtService
    {
        /// <summary>
        /// Creates jwt token for user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>Token.</returns>
        string GetJwtToken(User user);
    }
}
