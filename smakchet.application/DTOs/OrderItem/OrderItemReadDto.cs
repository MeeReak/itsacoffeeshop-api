namespace smakchet.application.DTOs.OrderItem
{
    public class OrderItemReadDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Size { get; set; }
        public string? Note { get; set; }
    }
}
