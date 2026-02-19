using smakchet.application.Constants.Enum;
using smakchet.application.Validation;

namespace smakchet.application.DTOs.OrderItem
{
    public class OrderItemUpdateDto
    {

        public int Quantity { get; set; }

        [EnumValidation(typeof(OrderItemSizeEnum))]
        public OrderItemSizeEnum? Size { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
