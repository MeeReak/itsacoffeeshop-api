using smakchet.application.Constants.Enum;
using smakchet.application.Validation;

namespace smakchet.application.DTOs.OrderItem
{
    public class OrderItemUpdateDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public string Number { get; set; } = string.Empty;

        [EnumValidation(typeof(OrderItemSizeEnum))]
        public OrderItemSizeEnum? Size { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
