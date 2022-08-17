using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatalogWebApi
{
    [ApiController]
    [Route("catalog/api/[controller]")]
    public class AccountController : BaseController<AccountDto, Account>
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService, IMapper mapper) : base(accountService, mapper)
        {
            this._accountService = accountService;
        }


        [HttpPost("register")]
        [AutomaticRetry(Attempts = 5, DelaysInSeconds = new int[] { 2, 2, 2, 2, 2 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public new async Task<IActionResult> CreateAsync([FromQuery] AccountDto resource)
        {
            var result = await _accountService.InsertAsync(resource);
            BackgroundJob.Enqueue(() => _accountService.SendEmail(resource, "Registration" ,"Successfully Registered"));

            if (!result.Success)
                return BadRequest(result);

            return StatusCode(201, result);
        }

        [HttpGet("my-account")]
        [Authorize]
        public async Task<IActionResult> GetUserDetail()
        {
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            return await base.GetByIdAsync(int.Parse(userId));
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> UpdatePasswordAsync([FromQuery] UpdatePasswordRequest resource)
        {

            // Check if the id belongs to me
            var identifier = (User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;

            // Checking duplicate password
            if (resource.OldPassword.Equals(resource.NewPassword))
                return BadRequest(new BaseResponse<AccountDto>("New password must be different."));

            var result = await _accountService.UpdatePasswordAsync(int.Parse(userId), resource);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete-my-account")]
        [Authorize]
        public new async Task<IActionResult> DeleteAsync()
        {
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            return await base.DeleteAsync(int.Parse(userId));
        }
    }
}
