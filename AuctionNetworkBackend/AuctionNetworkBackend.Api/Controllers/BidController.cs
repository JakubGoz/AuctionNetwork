using AuctionNetworkBackend.Application.Requests.BidRequests.AddBid;
using AuctionNetworkBackend.Application.Requests.BidRequests.GetListingBids;
using AuctionNetworkBackend.Application.Requests.BidRequests.GetLoggedUserBids;


using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace AuctionNetworkBackend.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/bid")]
    public class BidController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BidController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add/{listingId}")]
        public async Task<IActionResult> AddBid([FromRoute] long listingId, [FromBody] AddBidRequest request)
        {
            if (listingId != request.ListingId)
                return BadRequest("ListingId in route does not match ListingId in body");

            await _mediator.Send(request);
            return Ok();
        }

        [HttpGet("listing/{listingId}")]
        public async Task<IActionResult> GetListingBids([FromRoute] long listingId)
        {
            var result = await _mediator.Send(new GetListingBidsRequest { ListingId = listingId });
            return Ok(result);
        }
        [HttpGet("myitems")]
        public async Task<IActionResult> GetUsersBids()
        {
            var result = await _mediator.Send(new GetLoggedUserBidsRequest { });
            return Ok(result);
        }

    }
}
