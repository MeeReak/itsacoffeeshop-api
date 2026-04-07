using smakchet.application.DTOs.Variation;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IVariation
{
    public interface IVariationMapper : IMapper<Variation, VariationReadDto, VariationDto, VariationUpdateDto>
    {
    }
}
