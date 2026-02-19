using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.OrderItem;
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
                Type = (int)dto.Type!,
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
                Tax = entity.Tax,
                Subtotal = entity.Subtotal,
                CashierId = entity.CashierId,
                Total = entity.Total,
                CreatedAt = entity.CreatedAt,
                Items = entity.OrderItems
                    .Select(item => new OrderItemReadDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Size = item.Size,
                        Note = item.Note
                    })
                    .ToList()
            };
        }

        public void UpdateEntity(Order entity, OrderUpdateDto dto)
        {
            entity.Type = (int)dto.Type!;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
