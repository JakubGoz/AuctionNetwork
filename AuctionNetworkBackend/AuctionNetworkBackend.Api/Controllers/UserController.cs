using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuctionNetworkBackend.Application.Requests.UserRequests.LoginUser;
using AuctionNetworkBackend.Application.Requests.UserRequests.RegisterUser;
using AuctionNetworkBackend.Application.Requests.UserRequests.VerifyLoginUser;
using AuctionNetworkBackend.Application.Requests.UserRequests.VerifyRegisterUser;
namespace AuctionNetworkBackend.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Checks if email is not taken and sends verification token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("verify-register")]
        public async Task<IActionResult> VerifyRegisterUser(VerifyRegisterUserRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        /// <summary>
        /// Creates new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        /// <summary>
        /// Checks email and passwords then sends verification code.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        /// <summary>
        /// Checks token during logging in.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("verify-login")]
        public async Task<IActionResult> VerifyLoginUser(VerifyLoginUserRequest request)
        {
            var token = await _mediator.Send(request);
            return Ok(token);
        }
    }
}
