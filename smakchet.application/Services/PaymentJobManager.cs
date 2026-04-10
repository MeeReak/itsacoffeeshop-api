using System.Collections.Concurrent;
using smakchet.application.Interfaces.IPayment;

namespace smakchet.application.Services
{
    public class PaymentJobManager : IPaymentJobManager
    {
        private readonly ConcurrentDictionary<int, CancellationTokenSource> _jobs = new();

        public void RegisterJob(int paymentId, CancellationTokenSource cts)
        {
            _jobs.TryAdd(paymentId, cts);
        }

        public void RemoveJob(int paymentId)
        {
            if (_jobs.TryRemove(paymentId, out var cts))
            {
                // We don't dispose here because the worker might still be using the linked token
                // and it's better to let the worker's scope handle it or just rely on GC
                // since we want to avoid race conditions where the worker is about to use it.
            }
        }

        public void CancelJob(int paymentId)
        {
            if (_jobs.TryRemove(paymentId, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
}
