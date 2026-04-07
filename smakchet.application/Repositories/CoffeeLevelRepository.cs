using smakchet.application.Interfaces.ICoffeeLevel;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class CoffeeLevelRepository(SmakchetContext context) : BaseRepository<CoffeeLevel>(context), ICoffeeLevelRepository
{
}