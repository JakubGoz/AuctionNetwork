using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Shared.Exceptions;
using MediatR;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.PaymentRequests.GetPaymentIntent
{
    public class GetPaymentIntentRequestHandler : IRequestHandler<GetPaymentIntentRequest, GetPaymentIntentDto>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentIntentRequestHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<GetPaymentIntentDto> Handle(GetPaymentIntentRequest request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentById(request.PaymentId)
                ?? throw new NotFoundException("Payment not found");

            if (string.IsNullOrEmpty(payment.StripePaymentIntentId))
            {
                throw new NotFoundException("Stripe PaymentIntent ID not found for this payment.");
            }

            // 🔹 Pobieramy client_secret z Stripe API
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(payment.StripePaymentIntentId);

            return new GetPaymentIntentDto { ClientSecret = paymentIntent.ClientSecret };
        }
    }
}