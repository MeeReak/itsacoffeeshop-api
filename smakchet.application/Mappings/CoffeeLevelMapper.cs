using smakchet.application.DTOs.CoffeeLevel;
using smakchet.application.Interfaces.ICoffeeLevel;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class CoffeeLevelMapper : ICoffeeLevelMapper
    {
        public CoffeeLevelReadDto ToReadDto(CoffeeLevel entity)

        {
            return new CoffeeLevelReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt
            };
        }

        public CoffeeLevel ToEntity(CoffeeLevelDto dto)

        {
            return new CoffeeLevel
            {
                Name = dto.Name,
                CreatedAt = DateTime.Now
            };
        }

        public void UpdateEntity(CoffeeLevel entity, CoffeeLevelUpdateDto dto)

        {
            entity.Name = dto.Name;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
