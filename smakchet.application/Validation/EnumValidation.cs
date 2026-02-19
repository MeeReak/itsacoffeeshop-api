using System.ComponentModel.DataAnnotations;

namespace smakchet.application.Validation
{
    public class EnumValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValidationAttribute(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("Type must be an enum");
            _enumType = enumType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"{validationContext.DisplayName} is required.");

            if (!Enum.IsDefined(_enumType, value))
            {
                var allowedValues = string.Join(", ", Enum.GetValues(_enumType)
                    .Cast<Enum>()
                    .Select(e => $"{Convert.ToInt32(e)} = {e}"));

                return new ValidationResult(
                    $"Invalid value '{value}' for {validationContext.DisplayName}. Allowed values: {allowedValues}"
                );
            }

            return ValidationResult.Success;
        }
    }
}