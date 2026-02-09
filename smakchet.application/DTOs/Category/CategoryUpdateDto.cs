using smakchet.application.Constants.Category;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.Category
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = CategoryMessageConstant.RequiredName)]
        [StringLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = CategoryMessageConstant.RequiredDisplayNumber)]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
