using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.UserRequests.GetUserShortInfo
{
    public class GetUserShortInfoRequestHandler : IRequestHandler<GetUserShortInfoRequest, GetUserShortInfoDto>
    {
        private readonly IUserContextService _userContextService;
        private readonly IUserRepository _userRepository;

        public GetUserShortInfoRequestHandler(IUserContextService userContextService, IUserRepository userRepository)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
        }

        public async Task<GetUserShortInfoDto> Handle(GetUserShortInfoRequest request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId()
                         ?? throw new BadRequestException("User is not logged in");

            var user = await _userRepository.GetUserById(userId)
                       ?? throw new NotFoundException("User not found");

            return new GetUserShortInfoDto
            {
                UserId = user.Id,
                UserName = user.UserName
            };
        }
    }
}
