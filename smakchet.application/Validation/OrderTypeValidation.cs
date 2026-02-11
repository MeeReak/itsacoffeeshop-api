using smakchet.application.Constants;
using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.Validation
{
    public class OrderTypeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not OrderTypeEnum size)
            {
                return new ValidationResult(OrderMessageConstant.RequiredSize);
            }

            if (!Enum.IsDefined(typeof(OrderTypeEnum), size))
            {
                var allowedValues = string.Join(", ", Enum.GetNames(typeof(OrderTypeEnum)));
                return new ValidationResult(
                    string.Format(
                        ErrorMessageConstants.InvalidEnumValue,
                        size,
                        allowedValues
                    )
                );
            }

            return ValidationResult.Success;
        }
    }
}