using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Domain.Enums;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.CreateListing
{
    internal class CreateListingRequestHandler : IRequestHandler<CreateListingRequest>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        public CreateListingRequestHandler(IListingRepository listingRepository,IUserContextService userContextService,ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _listingRepository = listingRepository;
            _userContextService = userContextService;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }
        public async Task Handle(CreateListingRequest request, CancellationToken cancellationToken)
        {
            // Pobranie ID zalogowanego użytkownika
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new UnauthorizedException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
            ?? throw new NotFoundException("User was not found");

            // Sprawdzenie, czy kategoria istnieje
            var category = await _categoryRepository.GetCategoryById(request.CategoryId)
                ?? throw new NotFoundException("Category not found");

            // Pobranie instancji ListingStatus dla statusu "Active"
            var listingStatus = await _listingRepository.GetListingStatusById((long)ListingStatuses.Active);

            if (listingStatus == null)
            {
                throw new NotFoundException("Listing status 'Active' not found.");
            }

            // Tworzenie nowego ogłoszenia
            var listing = new Listing
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                BuyNowPrice = request.BuyNowPrice,
                StartDate = DateTime.UtcNow,
                EndDate = request.EndDate,
                IsAuction = request.IsAuction,
                Status = listingStatus,
                CategoryId = request.CategoryId,
                Seller = loggedUser
            };
            if (request.ListingPicture != null)
            {
                using var memoryStream = new MemoryStream();
                await request.ListingPicture.CopyToAsync(memoryStream);
                var listingPicture = new Photo
                {
                    Data = memoryStream.ToArray(),
                    ContentType = request.ListingPicture.ContentType
                };

                listing.Photo = listingPicture;
                listing.PhotoId = listingPicture.Id;

            }

            await _listingRepository.Create(listing);

            if (loggedUser.Listings == null)
            {
                loggedUser.Listings = new List<Listing>();
            }
            loggedUser.Listings.Add(listing);

            // Zaktualizowanie użytkownika w repozytorium
            await _userRepository.Update(loggedUser);
        }
    }
}
