using AutoMapper;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebApi
{
    [Route("protein/v1/api/[controller]")]
    [ApiController]
    public class ColorController : BaseController<ColorDto, Color>
    {
        public ColorController(IColorService ColorService, IMapper mapper) : base(ColorService, mapper)
        {
        }
    }
}
