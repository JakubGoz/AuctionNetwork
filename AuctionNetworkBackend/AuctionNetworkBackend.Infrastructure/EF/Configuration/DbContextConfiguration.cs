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
        IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);
        }
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .HasData(GetRoles());
        }
        public void Configure(EntityTypeBuilder<VerificationToken> builder)
        {
            builder
                .HasKey(x => x.Id);
        }
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .HasKey(x => x.Id);
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

        
    }
}
