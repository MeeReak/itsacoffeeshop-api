using smakchet.application.Constants.Enum;
using smakchet.application.Validation;

namespace smakchet.application.DTOs.Order
{
    public class OrderUpdateDto
    {
        [EnumValidation(typeof(OrderTypeEnum))]
        public OrderTypeEnum? Type { get; set; }
    }
}
