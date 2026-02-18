using Microsoft.EntityFrameworkCore;
using smakchet.application.Interfaces.IPayment;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class PaymentRepository(SmakchetContext context) : BaseRepository<Payment>(context), IPaymentRepository
{
    public async Task<Payment?> GetLatestPendingByOrderIdAsync(
        int orderId,
        CancellationToken cancellationToken)
    {
        return await context.Payments
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}