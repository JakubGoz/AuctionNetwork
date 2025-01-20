using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionNetworkBackend.Application.Repositories;
using AuctionNetworkBackend.Application.Services;
using AuctionNetworkBackend.Application.Requests.CategoryRequests;
using AuctionNetworkBackend.Shared.Exceptions;
using MediatR;

namespace AuctionNetworkBackend.Application.Requests.CategoryRequests.GetCategories
{
    public class GetCategoriesRequestHandler : IRequestHandler<GetCategoriesRequest, List<GetCategoriesDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesRequestHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<GetCategoriesDto>> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllCategories();

            var result = categories.Select(category => new GetCategoriesDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();
            
            return result;
        }
    }
}
