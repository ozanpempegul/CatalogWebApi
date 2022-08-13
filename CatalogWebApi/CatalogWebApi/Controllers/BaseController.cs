using AutoMapper;
using CatalogWebApi.Base;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebApi
{
    [ApiController]
    public class BaseController<Dto, Entity> : ControllerBase
    {
        private readonly IBaseService<Dto, Entity> _baseService;
        protected readonly IMapper Mapper;


        public BaseController(IBaseService<Dto, Entity> baseService, IMapper mapper)
        {
            this._baseService = baseService;
            this.Mapper = mapper;
        }

        [Route("GetAll")]
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            var result = await _baseService.GetAllAsync();

            if (!result.Success)
                return BadRequest(result);

            if (result.Response is null)
                return NoContent();

            return Ok(result);
        }

        [NonAction]
        public virtual async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _baseService.GetByIdAsync(id);

            if (!result.Success)
                return BadRequest(result);

            if (result.Response is null)
                return NoContent();

            return Ok(result);
        }

        [NonAction]
        public virtual async Task<IActionResult> CreateAsync([FromBody] Dto entity)
        {
            var result = await _baseService.InsertAsync(entity);

            if (result.Success)
                return StatusCode(201, result);

            return BadRequest(result);
        }

        [NonAction]
        public virtual async Task<IActionResult> UpdateAsync(int id, [FromBody] Dto entity)
        {
            var result = await _baseService.UpdateAsync(id, entity);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [NonAction]
        public virtual async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _baseService.RemoveAsync(id);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
       
    }
}
