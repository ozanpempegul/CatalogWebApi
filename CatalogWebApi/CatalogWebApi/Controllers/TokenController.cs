using CatalogWebApi.Base;
using CatalogWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CatalogWebApi
{
    [ApiController]
    [Route("catalog/api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenManagementService tokenManagementService;

        public TokenController(ITokenManagementService tokenManagementService) : base()
        {
            this.tokenManagementService = tokenManagementService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromQuery] TokenRequest tokenRequest)
        {
            string userAgent = Request.Headers["User-Agent"].ToString();
            var result = await tokenManagementService.GenerateTokensAsync(tokenRequest, DateTime.UtcNow, userAgent);

            if (result.Success)
            {
                Log.Information($"Role {result.Response.Role}: is logged in.");
                return Ok(result);
            }

            return Unauthorized(result);
        }
    }
}
