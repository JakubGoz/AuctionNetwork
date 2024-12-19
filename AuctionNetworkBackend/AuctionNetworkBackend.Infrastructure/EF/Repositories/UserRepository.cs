using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Infrastructure.EF.Contexts;

namespace AuctionNetworkBackend.Infrastructure.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuctionNetworkDbContext _dbContext;

        public UserRepository(AuctionNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User?> GetUserByEmail(string email)
            => await _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email);

        public async Task AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
