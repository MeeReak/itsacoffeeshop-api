namespace smakchet.application.DTOs.OrderItem
{
  public class OrderItemReadDto
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
    public string Number { get; set; } = string.Empty;
    public string? Note { get; set; }
    public ProductDetailReadDto? ProductDetails { get; set; } // Optional product info
    
    public int? SizeId { get; set; }
    public string? SizeName { get; set; }
    
    public int? IceId { get; set; }
    public string? IceName { get; set; }
    
    public int? SugarId { get; set; }
    public string? SugarName { get; set; }
    
    public int? CoffeeLevelId { get; set; }
    public string? CoffeeLevelName { get; set; }
    
    public int? VariationId { get; set; }
    public string? VariationName { get; set; }
  }

  public class ProductDetailReadDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
  }
}
