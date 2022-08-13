using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CatalogWebApi
{
    [Route("protein/v1/api/[controller]")]
    [ApiController]
    public class ProductController : BaseController<ProductDto, Product>
    {
        private readonly IProductService ProductService;

        public ProductController(IProductService ProductService, IMapper mapper) : base(ProductService, mapper)
        {
            this.ProductService = ProductService;
        }


        [HttpGet]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] int page, [FromQuery] int pageSize)
        {
            Log.Information($"{User.Identity?.Name}: get pagination Product.");

            QueryResource pagintation = new QueryResource(page, pageSize);

            var result = await ProductService.GetPaginationAsync(pagintation, null);

            if (!result.Success)
                return BadRequest(result);

            if (result.Response is null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost("pagination")]
        public async Task<IActionResult> GetPaginationWithFilterAsync([FromQuery] int page, [FromQuery] int pageSize, [FromBody] ProductDto filterResource)
        {
            Log.Information($"{User.Identity?.Name}: get pagination Product.");

            QueryResource pagintation = new QueryResource(page, pageSize);

            var result = await ProductService.GetPaginationAsync(pagintation, filterResource);

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

            return await base.GetByIdAsync(id);
        }

        [HttpPost]
        public new async Task<IActionResult> CreateAsync([FromBody] ProductDto resource)
        {
            Log.Information($"{User.Identity?.Name}: create a Product.");            

            var insertResult = await ProductService.InsertAsync(resource);

            if (!insertResult.Success)
                return BadRequest(insertResult);            

            return StatusCode(201, insertResult);
        }

        [HttpPut("{id:int}")]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductDto resource)
        {
            Log.Information($"{User.Identity?.Name}: update a Product with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }


        [HttpDelete("{id:int}")]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a Product with Id is {id}.");

            return await base.DeleteAsync(id);
        }

    }
}
