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
                OrderItems = entity.OrderItems != null && entity.OrderItems.Any()
                    ? [.. entity.OrderItems.Select(item => new OrderItemReadDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Note = item.Note,
                    Number = item.Number,
                    SizeId = item.SizeId,
                    SugarId = item.SugarLevelId,
                    CoffeeLevelId = item.CoffeeLevelId,
                    VariationId = item.VariationId,
                        ProductDetails = item.Product != null
                        ? new ProductDetailReadDto
                        {
                            Id = item.Product.Id,
                            Name = item.Product.Name,
                            Price = item.Product.Price,
                            ImageUrl = $"http://localhost:9000{item.Product.ImageUrl}"
                        }
                        : null
                })]
                        : []
            };
        }

        public void UpdateEntity(Order entity, OrderUpdateDto dto)
        {
            entity.Type = (int)dto.Type!;
            entity.UpdatedAt = DateTime.Now;
        }
    }
}