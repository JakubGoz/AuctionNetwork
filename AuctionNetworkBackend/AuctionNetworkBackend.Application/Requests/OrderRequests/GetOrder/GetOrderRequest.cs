using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.OrderRequests.GetOrder
{
    public class GetOrderRequest : IRequest<GetOrderDto>
    {
        public long OrderId { get; set; }
    }
}
