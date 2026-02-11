using System.ComponentModel.DataAnnotations;
using smakchet.application.Constants;
using smakchet.application.Constants.Enum;

namespace smakchet.application.Validation
{
    public class OrderItemSizeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string type || string.IsNullOrWhiteSpace(type))
            {
                return new ValidationResult(
                    ErrorMessageConstants.ValidationFailed
                );
            }

            if (!Enum.TryParse<OrderItemSizeEnum>(type, true, out _))
            {
                var allowedValues = string.Join(", ", Enum.GetNames(typeof(OrderItemSizeEnum)));

                return new ValidationResult(
                    string.Format(
                        ErrorMessageConstants.InvalidEnumValue,
                        type,
                        allowedValues
                    )
                );
            }

            return ValidationResult.Success;
        }
    }
}