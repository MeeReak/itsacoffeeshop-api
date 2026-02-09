using smakchet.application.DTOs.User;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IUser
{
    public interface IUserMapper : IMapper<User, UserReadDto, UserDto, UserUpdateDto>
    {
    }
}
