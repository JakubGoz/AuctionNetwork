using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuctionNetworkBackend.Application.Requests.UserRequests.GetUsers;
using AuctionNetworkBackend.Application.Requests.UserRequests.GetUserShortInfo;
using AuctionNetworkBackend.Application.Requests.UserRequests.LoginUser;
using AuctionNetworkBackend.Application.Requests.UserRequests.RegisterUser;
using AuctionNetworkBackend.Application.Requests.UserRequests.VerifyLoginUser;
using AuctionNetworkBackend.Application.Requests.UserRequests.VerifyRegisterUser;
using AuctionNetworkBackend.Application.Requests.UserRequests.VerifyPasswordReset;
using AuctionNetworkBackend.Application.Requests.UserRequests.PasswordReset;
using AuctionNetworkBackend.Application.Requests.UserRequests.ToggleLikeUser;

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

      
        [HttpPost("verify-register")]
        public async Task<IActionResult> VerifyRegisterUser(VerifyRegisterUserRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

       
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            var validator = new RegisterUserRequestValidator();

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("verify-login")]
        public async Task<IActionResult> VerifyLoginUser(VerifyLoginUserRequest request)
        {
            var token = await _mediator.Send(request);
            return Ok(token);
        }
        
        [HttpPost("verify-password-reset")]
        public async Task<IActionResult> VerifyPasswordReset(VerifyPasswordResetRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset(PasswordResetRequest request)
        {
            var validator = new PasswordResetRequestValidator();

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            await _mediator.Send(request);
            return Ok();
        }
        [Authorize]
        [HttpGet("is-logged-in")]
        public IActionResult IsLoggedIn()
        {
            return Ok();
        }
        [Authorize]
        [HttpGet("user-short-info")]
        public async Task<IActionResult> GetUserShortInfo()
        {
            var result = await _mediator.Send(new GetUserShortInfoRequest());
            return Ok(result);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPut("{userId}/toggle-like/{thumbUp}")]
        public async Task<IActionResult> ToggleUserLike([FromRoute] long userId, [FromRoute] bool thumbUp)
        {
            var request = new ToggleLikeUserRequest { UserId = userId, ThumbUp = thumbUp };

            await _mediator.Send(request);
            return Ok();
        }
        
    }
}
