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
using System.Text.RegularExpressions;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.DeleteListing
{
    public class DeleteListingRequestHandler : IRequestHandler<DeleteListingRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        private readonly IListingRepository _listingRepository;
        public DeleteListingRequestHandler(IListingRepository listingRepository, IUserRepository userRepository, IUserContextService userContextService)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
            _listingRepository = listingRepository;
        }
        public async Task Handle(DeleteListingRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");

            var listing = await _listingRepository.GetListingById(request.ListingId)
                ?? throw new NotFoundException("Listing was not found");

            if (listing.SellerId != loggedUserId && loggedUser.RoleId != (long)UserRoles.Admin)
            {
                throw new UnauthorizedException("User is unauthorized to delete this post");
            }

            await _listingRepository.Delete(listing);
        }
    }
}
