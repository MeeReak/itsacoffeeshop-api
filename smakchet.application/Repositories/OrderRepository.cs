using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces.IOrder;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class OrderRepository(SmakchetContext context)
    : BaseRepository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetOrderWithItemsAsync(
        int orderId,
        CancellationToken ct)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)

            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Size)

            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.IceLevel)

            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.SugarLevel)

            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.CoffeeLevel)

            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Variation)

            .FirstOrDefaultAsync(o => o.Id == orderId, ct);
    }
}