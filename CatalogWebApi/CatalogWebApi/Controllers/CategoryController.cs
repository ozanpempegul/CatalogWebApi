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
        public CategoryController(ICategoryService CategoryService, IMapper mapper) : base(CategoryService, mapper)
        {
        }
    }
}
