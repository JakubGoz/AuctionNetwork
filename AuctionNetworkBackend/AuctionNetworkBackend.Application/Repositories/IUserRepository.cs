using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Application.Requests.UserRequests.GetUsers;


namespace AuctionNetworkBackend.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(long id);
        Task<List<User>> GetUsers(GetUsersRequest request);
        Task AddUser(User user);

        Task Update(User user);
    }
}
