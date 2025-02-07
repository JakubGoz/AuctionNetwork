using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Pagination;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Domain.Enums;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetLoggedUserPurchases
{
    public class GetLoggedUserPurchasesRequestHandler : IRequestHandler<GetLoggedUserPurchasesRequest, PagedResult<GetLoggedUserPurchasesDto>>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        public GetLoggedUserPurchasesRequestHandler(IListingRepository listingRepository, IUserContextService userContextService, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _listingRepository = listingRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<PagedResult<GetLoggedUserPurchasesDto>> Handle(GetLoggedUserPurchasesRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
            ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");

            var listings = await _listingRepository.GetUserPurchases(loggedUserId);

            var pageSize = 6;

            var listingsDto = listings.Select(x => new GetLoggedUserPurchasesDto
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
                IsLiked = x.ListingReviews.FirstOrDefault(y => y.ReviewerId == loggedUserId) is not null,
            }).OrderByDescending(x => x.StartDate)
            .ToList();

            var pagedResult = new PagedResult<GetLoggedUserPurchasesDto>(listingsDto, listingsDto.Count, pageSize, request.PageNumber);

            return pagedResult;
        }
    }
}
