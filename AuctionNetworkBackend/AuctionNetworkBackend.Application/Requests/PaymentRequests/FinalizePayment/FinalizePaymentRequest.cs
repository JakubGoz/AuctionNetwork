using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.PaymentRequests.FinalizePayment
{
    public class FinalizePaymentRequest : IRequest<bool>
    {
        public long PaymentId { get; set; }
        public bool IsSuccesful { get; set; }
    }
}
