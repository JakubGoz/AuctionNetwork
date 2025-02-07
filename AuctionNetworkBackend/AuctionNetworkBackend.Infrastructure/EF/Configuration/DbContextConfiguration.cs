using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Domain.Enums;

namespace AuctionNetworkBackend.Infrastructure.EF.Configuration
{
    public class DbContextConfiguration :
        IEntityTypeConfiguration<User>,
        IEntityTypeConfiguration<Role>,
        IEntityTypeConfiguration<VerificationToken>,
        IEntityTypeConfiguration<Category>,
        IEntityTypeConfiguration<Listing>,
        IEntityTypeConfiguration<Bid>,
        IEntityTypeConfiguration<UserReview>,
        IEntityTypeConfiguration<ListingReview>,
        IEntityTypeConfiguration<ListingStatus>,
        IEntityTypeConfiguration<Photo>,
        IEntityTypeConfiguration<Order>, // Dodaj konfigurację dla Order
        IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
        .HasKey(x => x.Id);

            builder
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);

            builder.HasMany(u => u.Bids)
                .WithOne(b => b.Buyer)
                .HasForeignKey(b => b.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Listings)
                .WithOne(l => l.Seller)
                .HasForeignKey(l => l.SellerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserReviews)
                .WithOne(ur => ur.Reviewer)
                .HasForeignKey(ur => ur.ReviewerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .HasData(GetRoles());
        }
        public void Configure(EntityTypeBuilder<ListingStatus> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .HasData(GetStatuses());
        }
        public void Configure(EntityTypeBuilder<VerificationToken> builder)
        {
            builder
                .HasKey(x => x.Id);
        }

        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2);

            builder.HasMany(l => l.Bids)
                .WithOne(b => b.Listing)
                .HasForeignKey(b => b.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Buyer)
                .WithMany()
                .HasForeignKey(x => x.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Seller)
                .WithMany(u => u.Listings)
                .HasForeignKey(x => x.SellerId);

            builder.HasOne(x => x.Category)
                .WithMany(c => c.Listings)
                .HasForeignKey(x => x.CategoryId);

            builder.HasMany(l => l.ListingReviews)
                .WithOne(lr => lr.Listing)
                .HasForeignKey(lr => lr.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.ListingStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Photo)
                .WithOne()
                .HasForeignKey<Listing>(x => x.PhotoId);

            builder.HasOne(x => x.Winner)
                .WithMany()
                .HasForeignKey(x => x.WinnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(c => c.Listings)
                .WithOne(l => l.Category)
                .HasForeignKey(l => l.CategoryId);


            builder.HasData(GetCategories());
        }
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Price)
                .IsRequired();

            builder.Property(b => b.Date)
                .IsRequired();

            builder.HasOne(b => b.Listing)
                .WithMany(l => l.Bids)
                .HasForeignKey(b => b.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Buyer)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public void Configure(EntityTypeBuilder<UserReview> builder)
        {
            builder.HasKey(ur => ur.Id);

            builder.HasOne(ur => ur.Reviewer)
                .WithMany()
                .HasForeignKey(ur => ur.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ur => ur.ReviewedUser)
                .WithMany()
                .HasForeignKey(ur => ur.ReviewedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ur => ur.IsLike)
                .IsRequired();
        }
        public void Configure(EntityTypeBuilder<ListingReview> builder)
        {
            builder.HasKey(lr => lr.Id);

            builder.HasOne(lr => lr.Reviewer)
                .WithMany()
                .HasForeignKey(lr => lr.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lr => lr.Listing)
                .WithMany(l => l.ListingReviews)
                .HasForeignKey(lr => lr.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(lr => lr.Rating)
                .IsRequired()
                .HasDefaultValue(1); 


            builder.Property(lr => lr.Description)
                .HasMaxLength(500); // Opcjonalnie, ustalamy maksymalną długość

        }
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .HasOne(x => x.Listing)
                .WithOne(x => x.Photo)
                .HasForeignKey<Photo>(x => x.ListingId);
        }

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id); // Klucz główny

            builder.Property(o => o.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(o => o.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Street)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(o => o.PostalCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(o => o.PhoneNumber)
                .HasMaxLength(20);

            builder.HasOne(o => o.Listing)
                .WithMany() 
                .HasForeignKey(o => o.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id); // Klucz główny

            builder.Property(p => p.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(p => p.StripePaymentIntentId)
                .IsRequired(false)
                .HasMaxLength(255);
            
            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.HasOne(p => p.Order) 
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade); 
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>
            {
                new()
                {
                    Id = (long)UserRoles.User,
                    Name = "User"

                },
                new()
                {
                    Id = (long)UserRoles.Admin,
                    Name = "Admin"
                }
            };

            return roles;
        }
        private IEnumerable<ListingStatus> GetStatuses()
        {
            var statuses = new List<ListingStatus>
        {
            new()
            {
                Id = (long)ListingStatuses.Active,
                Name = "Active"

            },
            new()
            {
                Id = (long)ListingStatuses.Sold,
                Name = "Sold"
            },
            new()
            {
                Id = (long)ListingStatuses.Ended,
                Name = "Ended"

            }
        };

            return statuses;
        }
        private IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new() { Id = 1, Name = "Electronics" },
                new() { Id = 2, Name = "Fashion" },
                new() { Id = 3, Name = "Home & Garden" },
                new() { Id = 4, Name = "Sports & Outdoors" },
                new() { Id = 5, Name = "Automotive" },
                new() { Id = 6, Name = "Books & Media" },
                new() { Id = 7, Name = "Toys & Games" },
                new() { Id = 8, Name = "Health & Beauty" },
                new() { Id = 9, Name = "Industrial & Office Supplies" },
                new() { Id = 10, Name = "Music Instruments & Equipment" },
                new() { Id = 11, Name = "Food & Beverages" },
                new() { Id = 12, Name = "Hobbies & Crafts" }
            };
        }



    }
}
