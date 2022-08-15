using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CatalogWebApi.Base
{
    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                string source = value.ToString();

                // Validate pwd must be MD5 format
                if (source.Length < 8 || source.Length > 20)
                    return new ValidationResult("Invalid Password");

                return ValidationResult.Success;
            }
            catch (Exception)
            {
                return new ValidationResult("Invalid Password");
            }
        }
    }
}
