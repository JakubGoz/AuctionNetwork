using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AuctionNetworkBackend.Application.Repositories;

namespace AuctionNetworkBackend.Infrastructure.Services
{
    public class AuctionBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AuctionBackgroundService> _logger;

        public AuctionBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<AuctionBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var listingService = scope.ServiceProvider.GetRequiredService<IListingRepository>();
                        await listingService.CheckAndCloseAuctionsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in AuctionBackgroundService");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); 
            }
        }
    }
}
