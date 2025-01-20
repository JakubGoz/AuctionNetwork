using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace AuctionNetworkBackend.Application.Requests.CategoryRequests.GetCategories
{
    public class GetCategoriesRequest : IRequest<List<GetCategoriesDto>>
    {
    }
}
