using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuctionNetworkBackend.Application.Requests.PaymentRequests.CreatePayment;
using AuctionNetworkBackend.Application.Requests.PaymentRequests.FinalizePayment;
using AuctionNetworkBackend.Application.Requests.PaymentRequests.GetPaymentIntent;


namespace AuctionNetworkBackend.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment(CreatePaymentRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("finalize")]
        public async Task<IActionResult> FinalizePayment([FromBody] FinalizePaymentRequest request)
        {
            var result = await _mediator.Send(request);
            if (result)
            {
                return Ok("Payment finalized successfully.");
            }
            return BadRequest("Failed to finalize payment.");
        }
        [HttpGet("intent/{paymentId}")]
        public async Task<IActionResult> GetPaymentIntent(long paymentId)
        {
            var payment = await _mediator.Send(new GetPaymentIntentRequest { PaymentId = paymentId });
            return Ok(new { ClientSecret = payment.ClientSecret });
        }
    }
}
