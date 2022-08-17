using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Security.Claims;

namespace CatalogWebApi
{
    [Route("catalog/api/[controller]")]
    [ApiController]
    public class ProductController : BaseController<ProductDto, Product>
    {
        private readonly IProductService _productService;        
        private IMemoryCache _cache;

        public ProductController(IProductService productService, IMapper mapper, IMemoryCache cache) : base(productService, mapper)
        {
            this._productService = productService;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] int page, [FromQuery] int pageSize)
        {
            Log.Information($"{User.Identity?.Name}: get pagination Product.");

            QueryResource pagintation = new QueryResource(page, pageSize);

            var result = await _productService.GetPaginationAsync(pagintation, null);

            if (!result.Success)
                return BadRequest(result);

            if (result.Response is null)
                return NoContent();            

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public new async Task<IActionResult> GetByIdAsync(int id)
        {            
            Log.Information($"{User.Identity?.Name}: get a Product with Id is {id}.");
            var result = await _productService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("add-new-product")]
        [Authorize]
        public new async Task<IActionResult> CreateAsync([FromQuery] ProductDto resource, IFormFile? image)
        {
            Log.Information($"{User.Identity?.Name}: create a Product.");

            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            var insertResult = await _productService.InsertAsync(resource, int.Parse(userId), image);

            if (!insertResult.Success)
                return BadRequest(insertResult);            

            return StatusCode(201, insertResult);
        }

        [HttpPut("update-product/{id:int}")]
        [Authorize]
        public new async Task<IActionResult> UpdateAsync(int id, [FromQuery] ProductDto resource)
        {
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            Log.Information($"{User.Identity?.Name}: update a Product with Id is {id}.");
            var updateResult = await _productService.UpdateAsync(id, resource, int.Parse(userId));
            return StatusCode(202, updateResult);
        }


        [HttpDelete("delete-product/{id:int}")]
        [Authorize]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a Product with Id is {id}.");

            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;

            var result = await _productService.RemoveAsync(id, int.Parse(userId));

            return Ok();
        }


        
        [HttpGet("get-by-category-id/{categoryId}")]
        public new async Task<IActionResult> GetAllByCategoryIdAsync(int categoryId)
        {
            
            BaseResponse<IEnumerable<ProductDto>> default_cache;
            string productListCacheKey = "productListCacheKey";
            var result = new List<ProductDto>();

            if (_cache.TryGetValue(productListCacheKey, out default_cache))
            {
                result = default_cache.Response.Where(x => x.CategoryId == categoryId).ToList();
            }
            else
            {

                // cache has every result but only the requested result will be shown
                default_cache = await _productService.GetAllAsync();
                result = default_cache.Response.Where(x => x.CategoryId == categoryId).ToList();

                if (!default_cache.Success)
                    return BadRequest(default_cache);

                if (default_cache.Response is null)
                    return NoContent();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);

                _cache.Set(productListCacheKey, default_cache, cacheEntryOptions);
            }

            return Ok(result);
        }

        [HttpGet("my-products")]
        public new async Task<IActionResult> GetAllMyProductsAsync()
        {

            BaseResponse<IEnumerable<ProductDto>> myProducts;
            string AllMyProductsListCacheKey = "AllMyProductsListCacheKey";
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;

            if (_cache.TryGetValue(AllMyProductsListCacheKey, out myProducts))
            {
                
            }
            else
            {
                myProducts = await _productService.GetAllMyProductsAsync(int.Parse(userId));
                

                if (!myProducts.Success)
                    return BadRequest(myProducts);

                if (myProducts.Response is null)
                    return NoContent();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);

                _cache.Set(AllMyProductsListCacheKey, myProducts, cacheEntryOptions);
            }

            return Ok(myProducts);
        }
    }
}
