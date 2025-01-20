using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Infrastructure.EF.Configuration;

namespace AuctionNetworkBackend.Infrastructure.EF.Contexts
{
    public class AuctionNetworkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<VerificationToken> VerificationTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<UserReview> UserReviews { get; set; } 
        public DbSet<ListingReview> ListingReviews { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<ListingStatus> ListingsStatus { get; set; }
        public AuctionNetworkDbContext(DbContextOptions<AuctionNetworkDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var configuration = new DbContextConfiguration();

            modelBuilder.ApplyConfiguration<User>(configuration);
            modelBuilder.ApplyConfiguration<Role>(configuration);
            modelBuilder.ApplyConfiguration<VerificationToken>(configuration);
            modelBuilder.ApplyConfiguration<Category>(configuration);
            modelBuilder.ApplyConfiguration<Listing>(configuration);
            modelBuilder.ApplyConfiguration<Bid>(configuration);
            modelBuilder.ApplyConfiguration<UserReview>(configuration);
            modelBuilder.ApplyConfiguration<ListingReview>(configuration);
            modelBuilder.ApplyConfiguration<ListingStatus>(configuration);
        }

    }
}
