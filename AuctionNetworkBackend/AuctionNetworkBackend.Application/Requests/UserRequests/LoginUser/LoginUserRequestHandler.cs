using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.LoginUser
{
    public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ISmtpService _smtpService;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly IPasswordHasher<VerificationToken> _tokenHasher;

        public LoginUserRequestHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, ISmtpService smtpService, IVerificationTokenRepository tokenRepository, IPasswordHasher<VerificationToken> tokenHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _smtpService = smtpService;
            _tokenRepository = tokenRepository;
            _tokenHasher = tokenHasher;
        }

        public async Task Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email.ToLower())
                       ?? throw new BadRequestException($"Invalid email or password");

            var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (isPasswordCorrect == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException($"Invalid email or password");
            }

            var tokenValue = new Random().Next(10000, 99999).ToString();

            var token = new VerificationToken()
            {
                Email = request.Email,
                Created = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddMinutes(5),
            };

            var tokenHash = _tokenHasher.HashPassword(token, tokenValue);

            token.TokenHash = tokenHash;

            await _tokenRepository.AddToken(token);

            await _smtpService.SendMessage(request.Email.ToLower(), "Hello " + request.Email, tokenValue);
        }
    }
}
