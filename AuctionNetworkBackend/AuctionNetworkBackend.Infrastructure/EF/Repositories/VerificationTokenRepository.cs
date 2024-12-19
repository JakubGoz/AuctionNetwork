using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Infrastructure.EF.Contexts;
using AuctionNetworkBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionNetworkBackend.Infrastructure.EF.Repositories
{
    public class VerificationTokenRepository : IVerificationTokenRepository
    {
        private readonly AuctionNetworkDbContext _dbContext;

        public VerificationTokenRepository(AuctionNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddToken(VerificationToken token)
        {
            await _dbContext.VerificationTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<VerificationToken?> GetTokenByUserEmail(string email)
            => await _dbContext.VerificationTokens.OrderByDescending(x => x.Created).FirstOrDefaultAsync(x => x.Email == email);
    }
}
