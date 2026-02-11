using smakchet.application.Constants.Enum;
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
                Number = dto.Number,
                Type = (int)dto.Type,
                Status = (int)dto.Status,
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
                Status = (OrderStatusEnum)entity.Status,
                Type = (OrderTypeEnum)entity.Type,
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
                        Size = (OrderItemSizeEnum)item.Size,
                        Note = item.Note
                    })
                    .ToList()
            };
        }

        public void UpdateEntity(Order entity, OrderUpdateDto dto)
        {
            entity.Status = (int)dto.Status;
            entity.Type = (int)dto.Type;
            entity.Tax = dto.Tax;
            entity.Subtotal = dto.Subtotal;
            entity.Total = dto.Total;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}
