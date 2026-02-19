using smakchet.application.DTOs.OrderItem;

namespace smakchet.application.DTOs.Order
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int Type { get; set; } 
        public int Status { get; set; } 
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public int CashierId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemReadDto>? Items { get; set; }
    }
}
