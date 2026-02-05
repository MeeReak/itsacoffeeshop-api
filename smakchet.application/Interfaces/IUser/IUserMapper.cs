using smakchet.application.DTOs.User;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IUser
{
    public interface IUserMapper
    {
        UserReadDto ToSource(User user);
        User ToEntity(UserDto dto);
        void UpdateEntity(User user, UserUpdateDto dto);
    }
}
