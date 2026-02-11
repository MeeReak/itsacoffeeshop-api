using smakchet.application.Constants.Enum;

namespace smakchet.application.DTOs.OrderItem
{
    public class OrderItemUpdateDto
    {

        public int Quantity { get; set; }

        public OrderItemSizeEnum Size { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
