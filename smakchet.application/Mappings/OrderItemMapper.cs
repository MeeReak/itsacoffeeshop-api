using smakchet.application.Constants.Enum;
using smakchet.application.DTOs.OrderItem;
using smakchet.application.Interfaces.IOrderItem;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class OrderItemMapper : IOrderItemMapper
    {
        public OrderItem ToEntity(OrderItemDto dto)
        {
            return new OrderItem
            {
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Size = (int)dto.Size,
                Note = dto.Note,
                CreatedAt = DateTime.UtcNow
            };
        }

        public OrderItemReadDto ToReadDto(OrderItem entity)
        {
            return new OrderItemReadDto
            {
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                Price = entity.Price,
                Quantity = entity.Quantity,
                Size = (OrderItemSizeEnum)entity.Size,
                Note = entity.Note,
            };
        }

        public void UpdateEntity(OrderItem entity, OrderItemUpdateDto dto)
        {
            entity.Quantity = dto.Quantity;
            entity.Size = (int)dto.Size;
            entity.Note = dto.Note;
        }
    }
}
