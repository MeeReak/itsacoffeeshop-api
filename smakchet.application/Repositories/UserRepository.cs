using smakchet.application.Interfaces.IUser;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class UserRepository(SmakchetContext context) : BaseRepository<User>(context), IUserRepository
{
    public IQueryable<User> Query()
    {
        return context.Users;
    }
}