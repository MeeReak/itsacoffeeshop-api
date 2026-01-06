using System.ComponentModel.DataAnnotations;
using smakchet.application.Constants.User;

namespace smakchet.application.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = UserMessageConstant.RequiredName)]
        [StringLength(50)]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = UserMessageConstant.InvalidEmailFormat)]
        [Required(ErrorMessage = UserMessageConstant.RequiredEmail)]
        public required string Email { get; set; }
    }
}
