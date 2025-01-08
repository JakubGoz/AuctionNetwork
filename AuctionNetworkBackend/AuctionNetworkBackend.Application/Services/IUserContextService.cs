using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AuctionNetworkBackend.Application.Services
{
    public interface IUserContextService
    {
        long? GetUserId();
        ClaimsPrincipal User { get; }
    }
}
