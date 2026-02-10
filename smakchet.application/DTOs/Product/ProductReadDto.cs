namespace smakchet.application.DTOs.Product
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public bool? IsFeatured { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
