using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.UpdateListing
{

    public class UpdateListingRequestHandler : IRequestHandler<UpdateListingRequest>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IUserContextService _userContextService;
        public UpdateListingRequestHandler(IUserRepository userRepository, IUserContextService userContextService, IPhotoRepository photoRepository, IListingRepository listingRepository)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
            _photoRepository = photoRepository;
            _listingRepository = listingRepository;
        }

        public async Task Handle(UpdateListingRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");

            var listing = await _listingRepository.GetListingById(request.ListingId)
                ?? throw new NotFoundException("Listing was not found");

            listing.Title = request.Title;
            listing.Description = request.Description;
            listing.EndDate = request.EndDate;
            listing.Price = request.Price;
            listing.BuyNowPrice = request.BuyNowPrice;
            listing.IsAuction = request.IsAuction;
            listing.CategoryId = request.CategoryId;


            

            if (request.ListingPicture != null)
            {
                using var memoryStream = new MemoryStream();
                await request.ListingPicture.CopyToAsync(memoryStream);
                var profilePicture = new Photo
                {
                    Data = memoryStream.ToArray(),
                    ContentType = request.ListingPicture.ContentType
                };
                await _photoRepository.Delete(listing.Photo!);
                listing.Photo = profilePicture;
            }
            await _listingRepository.Update(listing);
            await _userRepository.Update(loggedUser);
        }
    }
}
