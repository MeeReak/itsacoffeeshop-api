using smakchet.application.Constants.Product;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.Product
{
    public class ProductUpdateDto
    {
        [Required(ErrorMessage = ProductMessageConstant.RequiredName)]
        [StringLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = ProductMessageConstant.InvalidDisplayNumber)]
        [RegularExpression(@"^\d+$", ErrorMessage = ProductMessageConstant.InvalidDisplayNumber)]
        public int DisplayOrder { get; set; }

        [Required(ErrorMessage = ProductMessageConstant.RequiredPrice)]
        [RegularExpression(@"^\d{1,5}$|(?=^.{1,5}$)^\d+\.\d{0,2}$", ErrorMessage = ProductMessageConstant.InvalidPrice)]
        public decimal Price { get; set; }

        public bool IsFeatured { get; set; } = false;
    }
}
