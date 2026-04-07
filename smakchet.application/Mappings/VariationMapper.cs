using smakchet.application.DTOs.Variation;
using smakchet.application.Interfaces.IVariation;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class VariationMapper : IVariationMapper
    {
        public VariationReadDto ToReadDto(Variation entity)

        {
            return new VariationReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt
            };
        }

        public Variation ToEntity(VariationDto dto)

        {
            return new Variation
            {
                Name = dto.Name,
                CreatedAt = DateTime.Now
            };
        }

        public void UpdateEntity(Variation entity, VariationUpdateDto dto)

        {
            entity.Name = dto.Name;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
