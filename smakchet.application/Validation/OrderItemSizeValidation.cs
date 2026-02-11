using System.ComponentModel.DataAnnotations;
using smakchet.application.Constants.Enum;
using smakchet.application.Constants;
using smakchet.application.Constants.Order;

namespace smakchet.application.Validation
{
    public class OrderItemSizeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not OrderItemSizeEnum size)
            {
                return new ValidationResult(OrderMessageConstant.RequiredSize);
            }

            if (!Enum.IsDefined(typeof(OrderItemSizeEnum), size))
            {
                var allowedValues = string.Join(", ", Enum.GetNames(typeof(OrderItemSizeEnum)));
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