using smakchet.application.DTOs.Size;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.ISize
{
    public interface ISizeMapper : IMapper<Size, SizeReadDto, SizeDto, SizeUpdateDto>
    {
    }
}
