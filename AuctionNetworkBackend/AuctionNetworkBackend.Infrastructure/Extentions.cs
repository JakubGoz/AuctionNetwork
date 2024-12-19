using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Infrastructure.EF;
using AuctionNetworkBackend.Infrastructure.EF.Options;
using AuctionNetworkBackend.Infrastructure.Jwt;
using AuctionNetworkBackend.Infrastructure.Services;
using AuctionNetworkBackend.Shared.Options;

namespace AuctionNetworkBackend.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetOptions<SmtpOptions>("Smtp");
            services.AddSingleton(options);

            services.AddHttpContextAccessor();
            services.AddPostgres(configuration);
            services.AddJwt(configuration);
            services.AddCors(options => {
                options.AddPolicy("FrontEndClient", builder => {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
                });
            });
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ISmtpService, SmtpService>();
            services.AddScoped<IUserContextService, UserContextService>();

            return services;
        }
    }
}
