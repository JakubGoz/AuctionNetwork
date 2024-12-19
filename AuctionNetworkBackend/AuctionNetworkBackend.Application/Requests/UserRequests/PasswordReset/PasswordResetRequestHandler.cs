using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Requests.UserRequests.RegisterUser;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Domain.Enums;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.PasswordReset
{
    public class PasswordResetRequestHandler : IRequestHandler<PasswordResetRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly IPasswordHasher<VerificationToken> _tokenHasher;

        public PasswordResetRequestHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IVerificationTokenRepository tokenRepository, IPasswordHasher<VerificationToken> tokenHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenRepository = tokenRepository;
            _tokenHasher = tokenHasher;
        }

        public async Task Handle(PasswordResetRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email.ToLower())
                       ?? throw new BadRequestException($"User with email: {request.Email} does not exist.");

            var token = await _tokenRepository.GetTokenByUserEmail(request.Email.ToLower())
                        ?? throw new NotFoundException("Tokens not found");

            var result = _tokenHasher.VerifyHashedPassword(token, token.TokenHash, request.Token);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid token");
            }

            if (token.ValidTo < DateTime.UtcNow)
            {
                throw new BadRequestException("Token has expired");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.Update(user);
        }
    }
}
