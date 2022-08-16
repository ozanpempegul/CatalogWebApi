using AutoMapper;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatalogWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : BaseController<OfferDto, Offer>
    {
        private readonly IOfferService _offerService;
        private readonly IProductRepository productRepository;

        public OfferController(IOfferService offerService, IMapper mapper, IProductRepository productRepository) : base(offerService, mapper)
        {
            this._offerService = offerService;
            this.productRepository = productRepository;
        }

        [HttpPost]
        [Authorize]
        public new async Task<IActionResult> CreateAsync(int productId, int offeredPrice, bool isPercent)
        {
            if(!productRepository.GetIsOfferable(productId))
            {
                BadRequest("Product is not offerable");
            }
            OfferDto resource = new();
            resource.ProductId = productId;
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            resource.BidderId = int.Parse(userId);
            var result = await _offerService.InsertAsync(resource, offeredPrice, isPercent);

            if (!result.Success)
                return BadRequest(result);

            return StatusCode(201, result);
        }

        [HttpDelete]
        [Authorize]
        public new async Task<IActionResult> DeleteAsync(int id)
        {

            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            var result = await _offerService.RemoveAsync(id, int.Parse(userId));

            if (!result.Success)
                return BadRequest(result);

            return StatusCode(201, result);
        }
    }
}
