using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IOrder
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetByNumberAsync(string number, CancellationToken cancellationToken);
        Task<Order?> GetOrderWithItemsAsync(int orderId, CancellationToken ct);
    }
}
