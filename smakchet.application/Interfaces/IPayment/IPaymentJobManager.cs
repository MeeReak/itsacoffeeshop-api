using System.Threading;

namespace smakchet.application.Interfaces.IPayment
{
    public interface IPaymentJobManager
    {
        void RegisterJob(int paymentId, CancellationTokenSource cts);
        void RemoveJob(int paymentId);
        void CancelJob(int paymentId);
    }
}
