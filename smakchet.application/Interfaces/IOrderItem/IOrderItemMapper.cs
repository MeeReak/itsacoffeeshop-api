using smakchet.application.DTOs.OrderItem;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IOrderItem
{
    public interface IOrderItemMapper
    {
        OrderItemReadDto ToReadDto(OrderItem entity);
    }
}
