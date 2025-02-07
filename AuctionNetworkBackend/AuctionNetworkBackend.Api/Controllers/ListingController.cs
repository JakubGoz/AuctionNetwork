using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuctionNetworkBackend.Application.Requests.ListingRequests.CreateListing;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingsByCategory;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingDetailsById;
using AuctionNetworkBackend.Application.Requests.ListingRequests.AddListingReview;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetReviews;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListingPhoto;
using AuctionNetworkBackend.Application.Requests.ListingRequests.UpdateListing;
using AuctionNetworkBackend.Application.Requests.ListingRequests.DeleteListing;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetUsersListings;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetLoggedUserPurchases;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetLoggedUserSales;


using AuctionNetworkBackend.Domain.Entities;
using AuctionNetworkBackend.Application.Requests.ListingRequests.GetListings;


namespace AuctionNetworkBackend.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/listing")]
    public class ListingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ListingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateListing(CreateListingRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpGet("results")]
        [Authorize]
        public async Task<IActionResult> GetListings([FromQuery]GetListingsRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetListingsByCategory([FromRoute] long categoryId)
        {
            var result = await _mediator.Send(new GetListingsByCategoryRequest { CategoryId = categoryId });
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUsersListing([FromRoute] long userId)
        {
            var result = await _mediator.Send(new GetUsersListingsRequest { UserId = userId });
            return Ok(result);
        }

        [HttpGet("purchases")]
        public async Task<IActionResult> GetLoggedUserPurchases()
        {
            var result = await _mediator.Send(new GetLoggedUserPurchasesRequest { });
            return Ok(result);
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetLoggedUserSales()
        {
            var result = await _mediator.Send(new GetLoggedUserSalesRequest { });
            return Ok(result);
        }

        [HttpGet("{listingId}")]
        public async Task<IActionResult> GetListingById([FromRoute] long listingId)
        {
            var result = await _mediator.Send(new GetListingDetailsByIdRequest { ListingId = listingId });
            return Ok(result);
        }

        [HttpPost("{listingId}/review")]
        public async Task<IActionResult> AddListingReview([FromRoute] long listingId, [FromBody] AddListingReviewRequest request)
        {
            if (listingId != request.ListingId)
                return BadRequest("ListingId in route does not match ListingId in body");

            await _mediator.Send(request);
            return Ok();
        }

        [HttpGet("{listingId}/getreviews")]
        public async Task<IActionResult> GetReviewsByListingId([FromRoute] long listingId)
        {
            var result = await _mediator.Send(new GetReviewsRequest { ListingId = listingId });
            return Ok(result);
        }

        [HttpGet("{listingId}/listing-picture")]
        public async Task<IActionResult> GetProfilePicture([FromRoute] long listingId)
        {
            var request = new GetListingPhotoRequest { ListingId = listingId };

            var result = await _mediator.Send(request);
            return File(result.Data, result.ContentType);
        }

        [HttpPut("update-listing/{listingId}")]
        public async Task<IActionResult> UpdateListing([FromRoute] long listingId, [FromForm] UpdateListingRequest request)
        {
            var updatedRequest = new UpdateListingRequest(listingId)
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                BuyNowPrice = request.BuyNowPrice,
                EndDate = request.EndDate,
                CategoryId = request.CategoryId,
                IsAuction = request.IsAuction,
                ListingPicture = request.ListingPicture
            };

            await _mediator.Send(updatedRequest);
            return Ok();
        }


        [HttpDelete("{listingId}")]
        public async Task<IActionResult> DeleteListing([FromRoute] long listingId)
        {
            var request = new DeleteListingRequest { ListingId = listingId };

            await _mediator.Send(request);
            return Ok();
        }
    }
}
