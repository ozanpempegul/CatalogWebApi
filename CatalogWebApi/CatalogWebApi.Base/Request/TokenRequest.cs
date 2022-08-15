using System.ComponentModel.DataAnnotations;
namespace CatalogWebApi.Base
{
    public class TokenRequest
    {
        [Required]
        [MaxLength(125)]
        [UserNameAttribute]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        //[PasswordAttribute]
        public string Password { get; set; }
    }
}
