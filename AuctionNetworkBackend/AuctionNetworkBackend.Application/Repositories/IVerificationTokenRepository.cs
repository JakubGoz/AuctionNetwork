using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Domain.Entities;


namespace AuctionNetworkBackend.Application.Repositories
{
    public interface IVerificationTokenRepository
    {
        Task AddToken(VerificationToken token);
        Task<VerificationToken?> GetTokenByUserEmail(string email);
    }
}
