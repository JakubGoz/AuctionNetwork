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

namespace AuctionNetworkBackend.Application.Requests.BidRequests.GetListingBids
{
    public class GetListingBidsRequestHandler : IRequestHandler<GetListingBidsRequest, List<GetListingBidsDto>>
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IBidRepository _bidRepository;
        public GetListingBidsRequestHandler(IListingRepository listingRepository, IUserContextService userContextService, IUserRepository userRepository, IBidRepository bidRepository) 
        { 
            _listingRepository = listingRepository;
            _userContextService = userContextService;
            _userRepository = userRepository;
            _bidRepository = bidRepository;
        }
        public async Task<List<GetListingBidsDto>> Handle(GetListingBidsRequest request, CancellationToken cancellationToken)
        {
            var loggedUserId = _userContextService.GetUserId()
                ?? throw new BadRequestException("User is not logged in");

            var loggedUser = await _userRepository.GetUserById(loggedUserId)
                ?? throw new NotFoundException("User was not found");
            
            var bids = await _bidRepository.GetBidsByListingId(request.ListingId);

            var BidsDto = bids.Select(x => new GetListingBidsDto
            {
                Price = x.Price,
                UserUserName = x.Buyer.UserName,
                BidDate = x.Date
            }).OrderByDescending(x => x.Price)
            .ToList();

            return BidsDto;
        }
    }
}
