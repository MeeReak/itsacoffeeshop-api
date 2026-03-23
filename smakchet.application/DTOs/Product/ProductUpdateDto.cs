using Microsoft.AspNetCore.Http;
using smakchet.application.Constants.Product;
using smakchet.application.Validation;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.Product
{
    public class ProductUpdateDto
    {
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [RegularExpression(@"^\d+$", ErrorMessage = ProductMessageConstant.InvalidDisplayNumber)]
        public int DisplayOrder { get; set; }

        [RegularExpression(@"^\d{1,5}$|(?=^.{1,5}$)^\d+\.\d{0,2}$", ErrorMessage = ProductMessageConstant.InvalidPrice)]
        public decimal Price { get; set; }
        public string? Description { get; set; } = string.Empty;

        [AllowedFileTypes(new[] { ".jpg", ".jpeg", ".png", ".webp" })]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile? File { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public bool IsFeatured { get; set; } = false;
    }
}
