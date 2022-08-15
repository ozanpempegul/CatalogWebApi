using CatalogWebApi.Base;
using System.ComponentModel.DataAnnotations;

namespace CatalogWebApi.Dto
{
    public class AccountDto : BaseDto
    {
        [UserNameAttribute]
        [MinLength(3)]
        [MaxLength(50)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        //[PasswordAttribute]
        public string Password { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddressAttribute(ErrorMessage = "Invalid Email Address")]
        [MaxLength(500)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [MaxLength(10, ErrorMessage = "Invalid Phone Length")]
        public string Phone { get; set; }

        [DateOfBirth]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public Nullable<DateTime> DateOfBirth { get; set; }

    }
}
