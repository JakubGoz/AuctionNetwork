using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.OrderRequests.GetOrder
{
    public class GetOrderRequestHandler : IRequestHandler<GetOrderRequest, GetOrderDto>
    {
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        public GetOrderRequestHandler(IOrderRepository orderRepository, IUserContextService userContextService, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
        }

        public async Task<GetOrderDto> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
            ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");

            var order = await _orderRepository.GetOrderById(request.OrderId);

            var result = new GetOrderDto
            {
                OrderPrice = order.Price
            };
            return result;
        }
    }
}
