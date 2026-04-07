using smakchet.application.DTOs.Size;
using smakchet.application.Interfaces.ISize;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class SizeMapper : ISizeMapper
    {
        public SizeReadDto ToReadDto(Size entity)

        {
            return new SizeReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt
            };
        }

        public Size ToEntity(SizeDto dto)

        {
            return new Size
            {
                Name = dto.Name,
                CreatedAt = DateTime.Now
            };
        }

        public void UpdateEntity(Size entity, SizeUpdateDto dto)

        {
            entity.Name = dto.Name;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
