using LoginHW.Base;
using System.ComponentModel.DataAnnotations;

namespace CatalogWebApi.Dto
{
    public class AccountDto : BaseDto
    {
        [Required]
        public int Id { get; set; }

        
        [UserNameAttribute]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [PasswordAttribute]
        public string Password { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        [EmailAddressAttribute]
        [MaxLength(500)]
        public string Email { get; set; }


        [Display(Name = "Last Activity")]
        public DateTime LastActivity { get; set; }
        
    }
}
