using System.ComponentModel.DataAnnotations;
using smakchet.application.Constants.User;

namespace smakchet.application.DTOs.User
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = UserMessageConstant.RequiredName)]
        [StringLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;
    }
}
