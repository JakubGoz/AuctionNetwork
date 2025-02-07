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
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingsByCategory;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListings
{
    public class GetListingsRequestHandler : IRequestHandler<GetListingsRequest, PagedResult<GetListingsDto>>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public GetListingsRequestHandler(IListingRepository listingRepository, IUserContextService userContextService, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _listingRepository = listingRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<PagedResult<GetListingsDto>> Handle(GetListingsRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");

            var listings = await _listingRepository.GetListings(request);

            if (request.MinPrice.HasValue)
            {
                listings = listings.Where(l => l.Price > request.MinPrice.Value).ToList();
            }

            if (request.MaxPrice.HasValue)
            {
                listings = listings.Where(l => l.Price < request.MaxPrice.Value).ToList();
            }

            if (request.IsAuction.HasValue)
            {
                listings = listings.Where(l => l.IsAuction == request.IsAuction.Value).ToList();
            }

            if (request.CategoryId.HasValue)
            {
                listings = listings.Where(l => l.CategoryId == request.CategoryId).ToList();
            }

            if (!string.IsNullOrEmpty(request.SearchQuery))
            {
                var searchTerm = request.SearchQuery.RemoveDiacritics().ToLower();
                listings = listings.Where(l =>
                    l.Title
                     .RemoveDiacritics()
                     .ToLower()
                     .Split(' ') // Rozdziel tytuł na słowa
                     .Any(word => word.LevenshteinDistance(searchTerm) <= 2) // Sprawdź odległość Levenshteina
                ).ToList();
            }

            switch (request.OrderBy)
            {
                case 1:
                    listings = listings.OrderBy(l => l.Price).ToList(); 
                    break;
                case 2:
                    listings = listings.OrderByDescending(l => l.Price).ToList(); 
                    break;
                case 3:
                    listings = listings.OrderByDescending(l => l.ListingReviews.Any() ? l.ListingReviews.Average(r => r.Rating) : 0).ToList(); 
                    break;
                case 4:
                    listings = listings.OrderByDescending(l => l.StartDate).ToList(); 
                    break;
                case 5:
                    listings = listings.OrderBy(l => l.EndDate).ToList(); 
                    break;
                default:
                    
                    listings = listings.OrderBy(l => l.StartDate).ToList(); 
                    break;
            }

            var pageSize = 6;
            var listingsDto = listings.Select(x => new GetListingsDto
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
                    : 0, // Jeśli brak recenzji, średnia to 0
            }).ToList();

            var pagedResult = new PagedResult<GetListingsDto>(listingsDto, listingsDto.Count, pageSize, request.PageNumber);

            return pagedResult;
        }

    }
}
