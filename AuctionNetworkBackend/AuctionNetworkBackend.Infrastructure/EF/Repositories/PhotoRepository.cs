using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Infrastructure.EF.Contexts;

namespace SocialNetworkBackend.Infrastructure.EF.Repositories;

public class PhotoRepository : IPhotoRepository
{
    private readonly AuctionNetworkDbContext _dbContext;

    public PhotoRepository(AuctionNetworkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Photo?> GetPhotoByUserId(long listingId)
        => await _dbContext.Photos
            .FirstOrDefaultAsync(x => x.ListingId == listingId);
    public async Task Delete(Photo photo)
    {
        _dbContext.Photos.Remove(photo);
        await _dbContext.SaveChangesAsync();
    }
}