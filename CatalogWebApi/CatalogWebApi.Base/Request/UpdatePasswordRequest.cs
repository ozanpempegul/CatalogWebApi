using System.ComponentModel.DataAnnotations;

namespace CatalogWebApi.Base
{
    public class UpdatePasswordRequest
    {
        [Required]
        [PasswordAttribute]
        public string OldPassword { get; set; }

        [Required]
        [PasswordAttribute]
        public string NewPassword { get; set; }
    }
}
