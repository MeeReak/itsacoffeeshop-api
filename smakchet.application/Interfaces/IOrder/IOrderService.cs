using smakchet.application.DTOs;
using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.Success;

namespace smakchet.application.Interfaces.IOrder
{
    public interface IOrderService
    {
        Task<ResponsePagingDto<OrderReadDto>> GetOrderPagedAsync(PaginationQueryParams param);
        Task<OrderReadDto?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken);
        Task<OrderReadDto?> UpdateOrderAsync(int orderId, OrderUpdateDto orderDto, CancellationToken cancellationToken);
        Task<OrderReadDto> CreateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken);
        Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken);
        Task<OrderReadDto?> GetStatusOrderAsync(int orderId, CancellationToken cancellationToken);
        Task<OrderReadDto?> GetOrderItemByIdAsync(int orderId, CancellationToken cancellationToken);
    }
}
