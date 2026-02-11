using smakchet.application.DTOs.Order;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IOrder
{
    public interface IOrderMapper : IMapper<Order, OrderReadDto, OrderDto, OrderUpdateDto>
    {
    }
}
