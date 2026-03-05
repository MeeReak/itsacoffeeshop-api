using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using smakchet.application.Interfaces.IPayment;

namespace smakchet.application.Helpers
{
    public class PaymentStatusWorker(
        IBackgroundQueueService<PaymentStatusJob> queue,
        IServiceScopeFactory scopeFactory,
        ILogger<PaymentStatusWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Queue started");
            while (!stoppingToken.IsCancellationRequested)
            {
                var job = await queue.DequeueAsync(stoppingToken);

                _ = ProcessPayment(job, stoppingToken);
            }
        }

        private async Task ProcessPayment(PaymentStatusJob job, CancellationToken token)
        {
            logger.LogInformation("Worker started");
            using var scope = scopeFactory.CreateScope();
            var paymentService = scope.ServiceProvider
                .GetRequiredService<IPaymentService>();

            bool finished = false;

            while (!finished && !token.IsCancellationRequested)
            {
                finished = await paymentService.CheckStatusAsync(job.PaymentId, token);

                if (!finished)
                    await Task.Delay(5000, token); // check every 5 sec
            }

            //logger.LogInformation("Payment {PaymentId} completed", job.PaymentId);
            logger.LogInformation("Worker ended");
        }
    }
}
