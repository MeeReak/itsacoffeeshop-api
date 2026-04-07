using smakchet.application.DTOs.Ice;
using smakchet.application.Interfaces.IIce;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class IceMapper : IIceMapper
    {
        public IceReadDto ToReadDto(Ice entity)

        {
            return new IceReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt
            };
        }

        public Ice ToEntity(IceDto dto)

        {
            return new Ice
            {
                Name = dto.Name,
                CreatedAt = DateTime.Now
            };
        }

        public void UpdateEntity(Ice entity, IceUpdateDto dto)

        {
            entity.Name = dto.Name;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
