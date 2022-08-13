using AutoMapper;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebApi.Controllers
{
    [Route("protein/v1/api/[controller]")]
    [ApiController]
    public class BrandController : BaseController<BrandDto, Brand>
    {
        public BrandController(IBrandService BrandService, IMapper mapper) : base(BrandService, mapper)
        {
        }
    }
}
