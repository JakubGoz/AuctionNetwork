using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Infrastructure.EF.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Infrastructure.EF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AuctionNetworkDbContext _dbContext;
        public CategoryRepository(AuctionNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _dbContext.Categories
                                 .Include(c => c.Listings) // Wczytujemy powiązane oferty, jeśli potrzeba
                                 .ToListAsync();
        }
        public async Task<Category> GetCategoryById(long id)
        {
            return await _dbContext.Categories
                                 .Include(c => c.Listings) // Opcjonalnie, jeśli chcesz załadować Listings
                                 .FirstOrDefaultAsync(c => c.Id == id); // Wyszukiwanie po Id
        }
    }

}
