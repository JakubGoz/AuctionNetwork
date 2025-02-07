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
using Microsoft.Extensions.Hosting;

namespace AuctionNetworkBackend.Application.Requests.BidRequests.AddBid
{
    public class AddBidRequestHandler : IRequestHandler<AddBidRequest>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IBidRepository _bidRepository;


        public AddBidRequestHandler(IListingRepository listingRepository, IUserContextService userContextService, IUserRepository userRepository, IBidRepository bidRepository) 
        {
            _listingRepository = listingRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _bidRepository = bidRepository;
        }

        public async Task Handle(AddBidRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");
            
            var listing = await _listingRepository.GetListingById(request.ListingId)
                ?? throw new NotFoundException("Listing was not found");

            var existingBid = await _bidRepository.GetBidByUserIdAndListingId(loggedUserId, request.ListingId);

            loggedUser.Bids ??= new List<Bid>();
            listing.Bids ??= new List<Bid>();


            if (listing.EndDate < DateTime.UtcNow.AddMinutes(2))
            {
                listing.EndDate = DateTime.UtcNow.AddMinutes(2);
                await _listingRepository.Update(listing);
            }
            
            if (existingBid != null)
            {
                existingBid.Date= DateTime.UtcNow;
                existingBid.Price= request.Price;

                await _bidRepository.Update(existingBid);
            }
            else
            {
                var bid = new Bid
                {
                    
                    ListingId = listing.Id,
                    Listing = listing,
                    Buyer = loggedUser,
                    BuyerId = loggedUser.Id,
                    Date = DateTime.UtcNow

                };
                loggedUser.Bids.Add(bid);
                listing.Bids.Add(bid);

                await _listingRepository.Update(listing);
                await _userRepository.Update(loggedUser);
                await _bidRepository.Create(bid);
            }
            listing.Price = request.Price;
            await _listingRepository.Update(listing);




        }
    }
}
