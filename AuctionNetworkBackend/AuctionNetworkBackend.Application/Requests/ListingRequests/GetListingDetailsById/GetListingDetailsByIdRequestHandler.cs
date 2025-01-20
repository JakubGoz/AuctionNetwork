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

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingDetailsById
{
    public class GetListingDetailsByIdRequestHandler : IRequestHandler<GetListingDetailsByIdRequest, GetListingDetailsByIdDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IListingRepository _listingRepository;

        public GetListingDetailsByIdRequestHandler(IUserRepository userRepository, IUserContextService userContextService, ICategoryRepository categoryRepository, IListingRepository listingRepository)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
            _categoryRepository = categoryRepository;
            _listingRepository = listingRepository;
        }
        public async Task<GetListingDetailsByIdDto> Handle(GetListingDetailsByIdRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");

            var listing = await _listingRepository.GetListingById(request.ListingId);

            var seller = await _userRepository.GetUserById(listing.SellerId);

            var result = new GetListingDetailsByIdDto
            {
                Id = listing.Id,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                BuyNowPrice = listing.BuyNowPrice,
                StartDate = listing.StartDate,
                EndDate = listing.EndDate,
                ListingStatus = listing.Status.Name,
                IsAuction = listing.IsAuction,
                IsItMyListing = loggedUserId == seller.Id,
                SellerId = listing.SellerId,
                SellerUserName = seller.UserName,
                SellerEmail = seller.Email,
                SellerPhoneNumber = seller.PhoneNumber,
                CategoryName = listing.Category.Name,
                Bids = listing.Bids,
                ListingReviewsCount = listing.ListingReviews.Count,
                SellerReviewsCount = seller.UserReviews.Count
            };
            return result;
        }
    }
}