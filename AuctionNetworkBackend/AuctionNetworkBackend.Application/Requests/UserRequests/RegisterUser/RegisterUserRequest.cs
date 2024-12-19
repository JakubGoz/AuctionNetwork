using MediatR;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.RegisterUser
{
    public class RegisterUserRequest : IRequest
    {
        public required string Email { get; set; }

        public required string UserName { get; set; }

        public required string Token { get; set; }

        public required string Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }
    }
}
