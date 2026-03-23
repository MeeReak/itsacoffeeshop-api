using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace smakchet.application.Validation
{
    public class AllowedFileTypesAttribute(string[] extensions) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
                return ValidationResult.Success;

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!extensions.Contains(extension))
            {
                return new ValidationResult($"Allowed file types: {string.Join(", ", extensions)}");
            }

            return ValidationResult.Success;
        }
    }

    public class MaxFileSizeAttribute(long maxFileSize) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
                return ValidationResult.Success;

            if (file.Length > maxFileSize)
            {
                return new ValidationResult($"Maximum allowed file size is {maxFileSize / (1024 * 1024)} MB.");
            }

            return ValidationResult.Success;
        }
    }
}
