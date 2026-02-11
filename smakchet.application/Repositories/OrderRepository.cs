using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces.IOrder;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class OrderRepository(SmakchetContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetByNumberAsync(int number, CancellationToken cancellationToken)
    {
        return await context.Orders
            .FirstOrDefaultAsync(c => c.Number == number, cancellationToken);
    }

    public async Task<Order?> GetOrderWithItemsAsync(
        int orderId,
        CancellationToken ct)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);
    }
}