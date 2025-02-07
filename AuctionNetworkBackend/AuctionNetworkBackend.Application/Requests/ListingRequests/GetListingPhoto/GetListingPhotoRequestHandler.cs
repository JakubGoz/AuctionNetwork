using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Shared.Exceptions;

namespace AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingPhoto
{
    public class GetListingPhotoRequestHandler : IRequestHandler<GetListingPhotoRequest, GetListingPhotoDto>
    {
        private readonly IPhotoRepository _photoRepository;

        public GetListingPhotoRequestHandler(IPhotoRepository photoRepository) 
        { 
            _photoRepository = photoRepository;
        }
        public async Task<GetListingPhotoDto> Handle(GetListingPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.GetPhotoByUserId(request.ListingId)
                ?? throw new NotFoundException("Photo was not found");

            return new GetListingPhotoDto
            {
                ContentType = photo.ContentType,
                Data = photo.Data
            };
        }
    }
}
