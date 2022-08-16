using AutoMapper;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebApi
{
    [Route("protein/v1/api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController<CategoryDto, Category>
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, IMapper mapper) : base(categoryService, mapper)
        {
            this._categoryService = categoryService;
        }

        [HttpDelete]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }

        [HttpPost]
        public new async Task<IActionResult> CreateAsync([FromBody] CategoryDto resource)
        {
            return await base.CreateAsync(resource);
        }

        [HttpPut]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] CategoryDto resource)
        {
            return await base.UpdateAsync(id, resource);
        }
    }
}
