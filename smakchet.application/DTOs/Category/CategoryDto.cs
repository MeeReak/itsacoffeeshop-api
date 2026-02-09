using System.ComponentModel.DataAnnotations;
using smakchet.application.Constants.Category;

namespace smakchet.application.DTOs.Category
{
    public class CategoryDto
    {
        [Required(ErrorMessage = CategoryMessageConstant.RequiredName)]
        [StringLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = CategoryMessageConstant.RequiredDisplayNumber)]
        [RegularExpression(@"^\d+$", ErrorMessage = CategoryMessageConstant.InvalidDisplayNumber)]
        public int DisplayOrder { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}
