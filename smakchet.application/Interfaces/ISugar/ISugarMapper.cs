using smakchet.application.DTOs.Sugar;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.ISugar
{
    public interface ISugarMapper : IMapper<Sugar, SugarReadDto, SugarDto, SugarUpdateDto>
    {
    }
}
