using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.PaymentRequests.GetPaymentIntent
{
    public class GetPaymentIntentRequest : IRequest<GetPaymentIntentDto>
    {
        public long PaymentId { get; set; }
    }
}
