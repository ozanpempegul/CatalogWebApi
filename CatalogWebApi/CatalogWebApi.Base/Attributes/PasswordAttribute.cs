using System.ComponentModel.DataAnnotations;

namespace CatalogWebApi.Base
{
    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                string source = value.ToString();
                
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
