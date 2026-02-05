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
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedDate = user.CreatedAt
            };
        }

        public User ToEntity(UserDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public void UpdateEntity(User user, UserUpdateDto dto)
        {
            user.Name = dto.Name;
        }
    }
}
