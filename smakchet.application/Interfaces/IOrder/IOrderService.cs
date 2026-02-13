using smakchet.application.DTOs;
using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.OrderItem;
using smakchet.application.DTOs.Success;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IOrder
{
    public interface IOrderService
    {
        Task<ResponsePagingDto<OrderReadDto>> GetOrderPagedAsync(PaginationQueryParams param);
        Task<OrderReadDto?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken);
        Task<OrderReadDto?> UpdateOrderAsync(int orderId, OrderUpdateDto orderDto, CancellationToken cancellationToken);
        Task<OrderReadDto> CreateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken);
        Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken);
        Task<OrderReadDto?> UpdateStatueOrderAsync(int orderId, OrderStatusDto status, CancellationToken cancellationToken);
        Task<OrderReadDto?> GetOrderItemByIdAsync(int orderId, CancellationToken cancellationToken);
        Task AddItemAsync(int orderId, OrderItemDto itemDto, CancellationToken cancellationToken);
        Task RemoveItemAsync(int orderId, int itemId, CancellationToken cancellationToken);
        Task UpdateItemAsync(int orderId, int itemId, OrderItemUpdateDto itemDto, CancellationToken cancellationToken);
    }
}
