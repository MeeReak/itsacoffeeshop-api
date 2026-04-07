using smakchet.application.DTOs.Sugar;
using smakchet.application.Interfaces.ISugar;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class SugarMapper : ISugarMapper
    {
        public SugarReadDto ToReadDto(Sugar entity)

        {
            return new SugarReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt
            };
        }

        public Sugar ToEntity(SugarDto dto)

        {
            return new Sugar
            {
                Name = dto.Name,
                CreatedAt = DateTime.Now
            };
        }

        public void UpdateEntity(Sugar entity, SugarUpdateDto dto)

        {
            entity.Name = dto.Name;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
