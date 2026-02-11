using smakchet.application.Constants.Enum;
using smakchet.application.DTOs.OrderItem;

namespace smakchet.application.DTOs.Order
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public OrderTypeEnum Type { get; set; } 
        public OrderStatusEnum Status { get; set; } 
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public int CashierId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemReadDto>? Items { get; set; }
    }
}
