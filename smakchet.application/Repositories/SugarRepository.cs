using smakchet.application.Interfaces.ISugar;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class SugarRepository(SmakchetContext context) : BaseRepository<Sugar>(context), ISugarRepository
{
}