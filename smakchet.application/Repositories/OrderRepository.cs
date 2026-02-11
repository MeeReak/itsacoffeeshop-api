using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces.IOrder;
using smakchet.dal.Models;
using System.Threading;

namespace smakchet.application.Repositories;

public class OrderRepository(SmakchetContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetByNumberAsync(int number, CancellationToken cancellationToken)
    {
        return await context.Orders
            .FirstOrDefaultAsync(c => c.Number == number, cancellationToken);
    }

    p
}