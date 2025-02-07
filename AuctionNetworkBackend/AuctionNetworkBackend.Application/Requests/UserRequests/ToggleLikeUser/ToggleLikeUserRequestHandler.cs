using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Shared.Exceptions;
using AuctionNetworkBackend.Domain.Entities;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.ToggleLikeUser
{
    
    public class ToggleLikeUserRequestHandler : IRequestHandler<ToggleLikeUserRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        public ToggleLikeUserRequestHandler(IUserContextService userContextService, IUserRepository postRepository)
        {
            _userContextService = userContextService;
            _userRepository = postRepository;
        }
        public async Task Handle(ToggleLikeUserRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");
            
            var user = await _userRepository.GetUserById(request.UserId)
            ?? throw new NotFoundException("User was not found");

            var userLike = user.UserReviews.FirstOrDefault(x => x.ReviewerId == loggedUserId);

            if (userLike != null)
            {
                userLike.IsLike = request.ThumbUp;
                
            }
            else
            {
                    user.UserReviews.Add(new()
                    {
                        ReviewerId = loggedUserId,
                        IsLike = request.ThumbUp,
                        ReviewedUserId = user.Id
                    });   
            }
            await _userRepository.Update(user);
        }
    }
}
