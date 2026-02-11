using smakchet.application.DTOs.Order;
using smakchet.application.Interfaces.IOrder;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class OrderMapper : IOrderMapper
    {
        public Order ToEntity(OrderDto dto)
        {
            return new Order
            {
                Number = dto.Number,
                Type = dto.Type,
                Status = dto.Status,
                Subtotal = dto.Subtotal,
                Total = dto.Total,
                Tax = dto.Tax,
                CashierId = dto.CashierId
            };
        }

        public OrderReadDto ToReadDto(Order entity)
        {
            return new OrderReadDto
            {
                Id = entity.Id,
                Number = entity.Number,
                Status = entity.Status,
                Type = entity.Type,
                Tax = (decimal)entity.Tax,
                Subtotal = (decimal)entity.Subtotal,
                CashierId = entity.CashierId,
                Total = (decimal)entity.Total,
                CreatedAt = entity.CreatedAt
            };
        }

        public void UpdateEntity(Order entity, OrderUpdateDto dto)
        {
            entity.Status = dto.Status;
            entity.Type = dto.Type;
            entity.Tax = dto.Tax;
            entity.Subtotal = dto.Subtotal;
            entity.Total = dto.Total;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
