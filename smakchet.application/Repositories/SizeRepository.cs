using smakchet.application.Interfaces.ISize;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class SizeRepository(SmakchetContext context) : BaseRepository<Size>(context), ISizeRepository
{
}