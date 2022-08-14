using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using CatalogWebApi.Service;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace CatalogWebApi
{
    [ApiController]
    [Route("protein/v1/api/[controller]")]
    public class AccountController : BaseController<AccountDto, Account>
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService, IMapper mapper) : base(accountService, mapper)
        {
            this._accountService = accountService;
        }

        [Authorize]
        public override Task<IActionResult> GetAllAsync()
        {
            return base.GetAllAsync();
        }


        [HttpPost]        
        public new IActionResult CreateAsync([FromBody] AccountDto resource) // public new async Task<IActionResult>
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("mephistopeles11@gmail.com", "otetltwcpvlcrtwx"); // TO DO read it from a file
                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(resource.Email);
                msgObj.From = new MailAddress("mephistopeles11@gmail.com");
                msgObj.Subject = "Registration";
                msgObj.Body = "Successfully Registered";


                try
                {
                    BackgroundJob.Enqueue(() => _accountService.InsertAsync(resource));
                }
                catch
                {
                    return BadRequest();
                }

                client.Send(msgObj);

                return StatusCode(201);
            }
        }

        [HttpGet("GetUserDetail")]
        [Authorize]
        public async Task<IActionResult> GetUserDetail()
        {
            var userId = (User.Identity as ClaimsIdentity).FindFirst("AccountId").Value;
            return await base.GetByIdAsync(int.Parse(userId));
        }

        [HttpPut("change-password/{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdatePasswordAsync(int id, [FromBody] UpdatePasswordRequest resource)
        {           
            // Check if the id belongs to me
            var identifier = (User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!identifier.Equals(id.ToString()))
                return BadRequest(new BaseResponse<AccountDto>("Account_Not_Permitted"));

            // Checking duplicate password
            if (resource.OldPassword.Equals(resource.NewPassword))
                return BadRequest(new BaseResponse<AccountDto>("New password must be different."));

            var result = await _accountService.UpdatePasswordAsync(id, resource);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            return await base.DeleteAsync(id);
        }

    }
}
