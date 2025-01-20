using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionNetworkBackend.Application.Requests.CategoryRequests.GetCategories
{
    public class GetCategoriesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
