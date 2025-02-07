using AuctionNetworkBackend.Application.Requests.ListingRequests.CreateListing;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingDetailsById;
using AuctionNetworkBackend.Application.Requests.OrderRequests.CreateOrder;
using AuctionNetworkBackend.Application.Requests.OrderRequests.GetOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            var result  = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] long orderId)
        {
            var result = await _mediator.Send(new GetOrderRequest { OrderId = orderId });
            return Ok(result);
        }

    }
}
