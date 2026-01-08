using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IUser
{
    public interface IUserRepository : IBaseRepository<User>
    {
        IQueryable<User> Query();
    }
}
