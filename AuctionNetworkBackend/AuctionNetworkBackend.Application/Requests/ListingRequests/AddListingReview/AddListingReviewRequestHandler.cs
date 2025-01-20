using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Shared.Exceptions;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.AddListingReview
{
    public class AddListingReviewRequestHandler : IRequestHandler<AddListingReviewRequest>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;

        public AddListingReviewRequestHandler(IUserContextService userContextService, IListingRepository listingRepository)
        {
            _userContextService = userContextService;
            _listingRepository = listingRepository;
        }

        public async Task Handle(AddListingReviewRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new BadRequestException("Rating must be between 1 and 5");
            }

            var listing = await _listingRepository.GetListingById(request.ListingId)
                ?? throw new NotFoundException("Listing was not found");

            var existingReview = listing.ListingReviews.FirstOrDefault(x => x.ReviewerId == loggedUserId);

            if (existingReview != null)
            {
                existingReview.Rating = request.Rating;
                existingReview.Description = request.Description;
            }
            else
            {
                listing.ListingReviews.Add(new()
                {
                    ReviewerId = loggedUserId,
                    ListingId = listing.Id,
                    Rating = request.Rating,
                    Description = request.Description
                });
            }

            await _listingRepository.Update(listing);
        }
    }
}
