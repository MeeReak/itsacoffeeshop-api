using smakchet.application.DTOs.User;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IUser
{
    public interface IUserMapper
    {
        UserReadDto ToReadDto(User user);
        User ToEntity(UserDto dto);
        void UpdateEntity(User user, UserDto dto);
    }
}
