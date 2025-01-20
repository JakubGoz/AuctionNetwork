using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Domain.Enums;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetReviews
{
    public class GetReviewsRequestHandler : IRequestHandler<GetReviewsRequest, List<GetReviewsDto>>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;

        public GetReviewsRequestHandler(IListingRepository listingRepository, IUserContextService userContextService, IUserRepository userRepository)
        {
            _listingRepository = listingRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
        }
        public async Task<List<GetReviewsDto>> Handle(GetReviewsRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");

            var reviews = await _listingRepository.GetReviewsByListingId(request.ListingId);
            var reviewsDto = reviews.Select(x => new GetReviewsDto
            {
                ReviewerId = x.Id,
                ReviewerUserName = x.Reviewer.UserName,
                Rating = x.Rating,
                Description = x.Description
            }
            ).OrderByDescending(x => x.Rating)
            .ToList();
            return reviewsDto;

        }
    }
}
