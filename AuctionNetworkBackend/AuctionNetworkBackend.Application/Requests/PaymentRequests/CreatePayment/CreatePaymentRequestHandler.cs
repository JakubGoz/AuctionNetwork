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

namespace AuctionNetworkBackend.Application.Requests.PaymentRequests.CreatePayment
{
    public class CreatePaymentRequestHandler : IRequestHandler<CreatePaymentRequest, long>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;


        public CreatePaymentRequestHandler(IOrderRepository orderRepository, IUserContextService userContextService, IPaymentRepository paymentRepository, IUserRepository userRepository)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;

        }
        public async Task<long> Handle(CreatePaymentRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(request.OrderId)
                ?? throw new NotFoundException("Order not found");

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.Price * 100), 
                Currency = "pln",
                Metadata = new Dictionary<string, string>
        {
            { "OrderId", order.Id.ToString() }
        }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            var payment = new Payment
            {
                OrderId = request.OrderId,
                StripePaymentIntentId = paymentIntent.Id,
                PaymentStatus = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            var createdPayment = await _paymentRepository.AddPayment(payment);
            return createdPayment.Id;
        }

    }
}
