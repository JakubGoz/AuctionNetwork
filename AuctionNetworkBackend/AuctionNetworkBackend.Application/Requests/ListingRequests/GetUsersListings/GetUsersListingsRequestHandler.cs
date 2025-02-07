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
using AuctionNetworkBackend.Application.Pagination;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingsByCategory;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetUsersListings
{
    public class GetUsersListingsRequestHandler : IRequestHandler<GetUsersListingsRequest, PagedResult<GetUsersListingsDto>>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;

        public GetUsersListingsRequestHandler(IListingRepository listingRepository, IUserContextService userContextService, IUserRepository userRepository)
        {
            _listingRepository = listingRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
        }

        public async Task<PagedResult<GetUsersListingsDto>> Handle(GetUsersListingsRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");

            var listings = await _listingRepository.GetListingsBySellerId(request.UserId);

            var user = await _userRepository.GetUserById(request.UserId);
            var pageSize = 6;

            var listingsDto = listings.Select(x => new GetUsersListingsDto
            {
                SellerId = x.SellerId,
                SellerUserName = x.Seller.UserName,
                ListingId = x.Id,
                Title = x.Title,
                Price = x.Price,
                BuyNowPrice = x.BuyNowPrice,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ListingStatus = x.Status.Name,
                IsAuction = x.IsAuction,
                ListingReviewsCount = x.ListingReviews.Any() ? x.ListingReviews.Count : 0,
                ListingReviewsAvg = x.ListingReviews.Any()
                        ? x.ListingReviews.Average(y => y.Rating)
                        : 0,  // Jeśli brak recenzji, średnia to 0
                IsLiked = x.ListingReviews.FirstOrDefault(y => y.ReviewerId == loggedUserId) is not null
            }).OrderByDescending(x => x.EndDate)
            .ToList();

            var pagedResult = new PagedResult<GetUsersListingsDto>(listingsDto, listingsDto.Count, pageSize, request.PageNumber);

            return pagedResult;
        }
    }
}
