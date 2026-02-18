using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IPayment
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetLatestPendingByOrderIdAsync(int orderId, CancellationToken cancellationToken);
    }
}
