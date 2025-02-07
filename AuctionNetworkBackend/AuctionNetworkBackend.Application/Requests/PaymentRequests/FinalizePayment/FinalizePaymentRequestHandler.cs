using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Stripe;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Domain.Enums;
using AuctionNetworkBackend.Shared.Exceptions;


namespace AuctionNetworkBackend.Application.Requests.PaymentRequests.FinalizePayment
{
    public class FinalizePaymentRequestHandler : IRequestHandler<FinalizePaymentRequest, bool>
    {
        private readonly IUserContextService _userContextService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IListingRepository _listingRepository;

        public FinalizePaymentRequestHandler(IListingRepository listingRepository, IOrderRepository orderRepository, IUserContextService userContextService, IPaymentRepository paymentRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _listingRepository = listingRepository;
        }

        public async Task<bool> Handle(FinalizePaymentRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new UnauthorizedException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
            ?? throw new NotFoundException("User was not found");

            var listingStatus = await _listingRepository.GetListingStatusById((long)ListingStatuses.Sold);

            var payment = await _paymentRepository.GetPaymentById(request.PaymentId);

            var service = new PaymentIntentService();

            var paymentIntent = await service.GetAsync(payment.StripePaymentIntentId);

            if (paymentIntent.Status == "succeeded")
            {
                payment.PaymentStatus = "Completed";
                var order = await _orderRepository.GetOrderById(payment.OrderId);
                order.Status = "Completed";
                payment.Order.Listing.BuyerId = loggedUserId;
                payment.Order.Listing.Status = listingStatus;
                await _orderRepository.SaveChangesAsync();
                return true;
            }
            else if (paymentIntent.Status == "requires_action" || paymentIntent.Status == "processing")
            {
                throw new InvalidOperationException("Payment is still processing. Try again later.");
            }
            else if (paymentIntent.Status == "canceled")
            {
                throw new InvalidOperationException("Payment has been canceled.");
            }

            return false;
        }
    }
}
