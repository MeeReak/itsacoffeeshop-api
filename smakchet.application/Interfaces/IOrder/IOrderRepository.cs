using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IOrder
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetOrderWithItemsAsync(int orderId, CancellationToken ct);
    }
}
