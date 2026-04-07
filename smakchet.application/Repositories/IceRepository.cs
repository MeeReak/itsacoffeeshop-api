using smakchet.application.Interfaces.IIce;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class IceRepository(SmakchetContext context) : BaseRepository<Ice>(context), IIceRepository
{
}