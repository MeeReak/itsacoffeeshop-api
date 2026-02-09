using smakchet.application.Constants.Category;
using smakchet.application.Constants.User;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.User
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = UserMessageConstant.RequiredName)]
        [StringLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = UserMessageConstant.RequiredName)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
