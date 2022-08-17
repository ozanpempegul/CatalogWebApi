using AutoMapper;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatalogWebApi.Controllers
{
    [Route("catalog/api/[controller]")]
    [ApiController]
    public class OfferController : BaseController<OfferDto, Offer>
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService, IMapper mapper) : base(offerService, mapper)
        {
            this._offerService = offerService;            
        }

        [HttpPost("make-offer")]
        [Authorize]
        public new async Task<IActionResult> CreateAsync(int productId, int offeredPrice, bool isPercent)
        {
            OfferDto resource = new();
            resource.ProductId = productId;
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            resource.BidderId = int.Parse(userId);
            var result = await _offerService.InsertAsync(resource, offeredPrice, isPercent);

            if (!result.Success)
                return BadRequest(result);

            return StatusCode(201, result);
        }

        [HttpDelete("cancel-offer")]
        [Authorize]
        public new async Task<IActionResult> DeleteAsync(int id)
        {

            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            var result = await _offerService.RemoveAsync(id, int.Parse(userId));

            if (!result.Success)
                return BadRequest(result);

            return StatusCode(201, result);
        }

        [HttpGet("my-offers")]
        [Authorize]
        public new async Task<IActionResult> GetMyOffers()
        {
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            var result = await _offerService.GetMyOffersAsync(int.Parse(userId));

            if (!result.Success)
                return BadRequest(result);

            if (result.Response is null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("my-products-offers")]
        [Authorize]
        public new async Task<IActionResult> GetMyProductsOffers()
        {
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            var result = await _offerService.GetMyProductsOffersAsync(int.Parse(userId));

            if (!result.Success)
                return BadRequest(result);

            if (result.Response is null)
                return NoContent();

            return Ok(result);
        }

        [HttpDelete("sell-by-id/{offerId}")]
        [Authorize]
        public new async Task<IActionResult> SellAsync(int offerId)
        {
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            var result = await _offerService.SellAsync(int.Parse(userId), offerId);
            return Ok(result);
        }
    }
}
