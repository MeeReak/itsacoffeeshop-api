using smakchet.application.DTOs.User;
using smakchet.application.Interfaces.IUser;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class UserMapper : IUserMapper
    {
        public UserReadDto ToSource(User user)
        {
            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
            };
        }

        public User ToEntity(UserDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = dto.Password,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void UpdateEntity(User user, UserDto dto)
        {
            user.Name = dto.Name;
            user.Email = dto.Email;
        }
    }
}
