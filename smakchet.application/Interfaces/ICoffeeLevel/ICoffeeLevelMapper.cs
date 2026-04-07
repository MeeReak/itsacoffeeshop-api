using smakchet.application.DTOs.CoffeeLevel;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.ICoffeeLevel
{
    public interface ICoffeeLevelMapper : IMapper<CoffeeLevel, CoffeeLevelReadDto, CoffeeLevelDto, CoffeeLevelUpdateDto>
    {
    }
}
