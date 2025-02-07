using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Infrastructure.EF;
using AuctionNetworkBackend.Infrastructure.EF.Options;
using AuctionNetworkBackend.Infrastructure.EF.Repositories;
using AuctionNetworkBackend.Infrastructure.Jwt;
using AuctionNetworkBackend.Infrastructure.Services;
using AuctionNetworkBackend.Shared.Options;
using Microsoft.Extensions.Hosting;


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
            services.AddScoped<IListingRepository, ListingRepository>();
            services.AddHostedService<AuctionBackgroundService>();


            // Ustaw cykl życia ListingStatusBackgroundService na Scoped lub Transient
            //services.AddScoped<ListingStatusBackgroundService>(); // Jeśli chcesz używać Scoped
            //services.AddHostedService<ListingStatusBackgroundService>();

            return services;
        }
    }
}
