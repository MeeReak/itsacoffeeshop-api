using Microsoft.Extensions.Configuration;
using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.OrderItem;
using smakchet.application.Interfaces.IOrder;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class OrderMapper(IConfiguration configuration) : IOrderMapper
    {
        private readonly string _baseUrl = configuration["Minio:ServiceUrl"] ?? "http://localhost:9000";

        public Order ToEntity(OrderDto dto)
        {
            return new Order
            {
                Type = (int)dto.Type!,
                CashierId = dto.CashierId,
                OrderItems = dto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Note = item.Note,
                    Number = item.Number,
                    SizeId = item.SizeId,
                    IceLevelId = item.IceLevelId,
                    SugarLevelId = item.SugarLevelId,
                    CoffeeLevelId = item.CoffeeLevelId,
                    VariationId = item.VariationId
                }).ToList()
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
                    SizeName = item.Size?.Name,
                    IceId = item.IceLevelId,
                    IceName = item.IceLevel?.Name,
                    SugarId = item.SugarLevelId,
                    SugarName = item.SugarLevel?.Name,
                    CoffeeLevelId = item.CoffeeLevelId,
                    CoffeeLevelName = item.CoffeeLevel?.Name,
                    VariationId = item.VariationId,
                    VariationName = item.Variation?.Name,
                        ProductDetails = item.Product != null
                        ? new ProductDetailReadDto
                        {
                            Id = item.Product.Id,
                            Name = item.Product.Name,
                            Price = item.Product.Price,
                            ImageUrl = $"{_baseUrl}{item.Product.ImageUrl}"
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
