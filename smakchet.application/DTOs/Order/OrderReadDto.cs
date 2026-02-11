using smakchet.application.DTOs.OrderItem;

namespace smakchet.application.DTOs.Order
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public int CashierId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemReadDto>? Items { get; set; }
    }
}
