using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Domain.Enums;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.RegisterUser
{
    public class RegisterUserRequestHandler : IRequestHandler<RegisterUserRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly IPasswordHasher<VerificationToken> _tokenHasher;

        public RegisterUserRequestHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IVerificationTokenRepository tokenRepository, IPasswordHasher<VerificationToken> tokenHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenRepository = tokenRepository;
            _tokenHasher = tokenHasher;
        }

        public async Task Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var emailAlreadyExists = await _userRepository.GetUserByEmail(request.Email.ToLower());

            if (emailAlreadyExists is not null)
            {
                throw new BadRequestException($"User with email: {request.Email} already exists.");
            }

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

            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Country = request.Country,
                City = request.City,
                RoleId = (long)UserRoles.User,
                PasswordHash = ""
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.AddUser(user);
        }
    }
}
