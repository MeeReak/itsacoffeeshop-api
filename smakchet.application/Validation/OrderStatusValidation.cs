using smakchet.application.Constants;
using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.Validation
{
    public class OrderStatusValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not OrderStatusEnum size)
            {
                return new ValidationResult(OrderMessageConstant.RequiredSize);
            }

            if (!Enum.IsDefined(typeof(OrderStatusEnum), size))
            {
                var allowedValues = string.Join(", ", Enum.GetNames(typeof(OrderStatusEnum)));
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