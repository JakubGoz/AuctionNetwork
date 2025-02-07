using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Domain.Enums;
using AuctionNetworkBackend.Shared.Exceptions;
using AuctionNetworkBackend.Application.Requests.OrderRequests.CreateOrder;

namespace AuctionNetworkBackend.Application.Requests.OrderRequests.CreateOrder
{
    public class CreateOrderRequestHandler : IRequestHandler<CreateOrderRequest, long>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        public CreateOrderRequestHandler(IListingRepository listingRepository, IUserContextService userContextService, IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _listingRepository = listingRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }
        public async Task<long> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
               ?? throw new UnauthorizedException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
            ?? throw new NotFoundException("User was not found");

            var listing = await _listingRepository.GetListingById(request.ListingId);

            var order = new Order
            {
                UserId = loggedUserId,
                ListingId = request.ListingId,
                ProductName = listing.Title,
                Price = (listing.BuyNowPrice.HasValue && listing.ListingStatusId == 1)
                    ? listing.BuyNowPrice.Value
                    : listing.Price,
                Status = "Pending", // Początkowy status
                CreatedAt = DateTime.UtcNow,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Country = request.Country,
                City = request.City,
                Street = request.Street,
                PostalCode = request.PostalCode,
                PhoneNumber = request.PhoneNumber
            };

            var createdOrder = await _orderRepository.AddOrder(order);
            return createdOrder.Id; // Zwracamy ID utworzonego zamówienia
        }
    }
}
