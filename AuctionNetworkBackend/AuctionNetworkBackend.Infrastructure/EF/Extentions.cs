using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuctionNetworkBackend.Infrastructure.EF.Contexts;
using AuctionNetworkBackend.Infrastructure.EF.Options;
using AuctionNetworkBackend.Infrastructure.EF.Repositories;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Shared.Options;

namespace AuctionNetworkBackend.Infrastructure.EF
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {

            var options = configuration.GetOptions<PostgresOptions>("Postgres");

            services.AddDbContext<AuctionNetworkDbContext>(ctx
                => ctx.UseNpgsql(options.ConnectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();
            return services;
        }
    }
}
