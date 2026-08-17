using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is DateTime dateTime)
            {
                if(dateTime < DateTime.UtcNow)
                {
                    return new ValidationResult(
                    ErrorMessage ??
                    "Due date must be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
