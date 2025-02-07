using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.PaymentRequests.CreatePayment
{
    public class CreatePaymentRequest : IRequest<long>
    {
        public long OrderId { get; set; }
     
    }
}
