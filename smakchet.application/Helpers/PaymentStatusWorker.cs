using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using smakchet.application.Constants.Enum;
using smakchet.application.Interfaces.IPayment;

namespace smakchet.application.Helpers
{
    public class PaymentStatusWorker(
        IBackgroundQueueService<PaymentStatusJob> queue,
        IServiceScopeFactory scopeFactory,
        IPaymentJobManager jobManager,
        ILogger<PaymentStatusWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("PaymentStatusWorker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var job = await queue.DequeueAsync(stoppingToken);

                    // Create a linked token source so we can cancel this specific job
                    // while also respecting the worker's stopping token.
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    jobManager.RegisterJob(job.PaymentId, cts);

                    try
                    {
                        await ProcessPayment(job, cts.Token);
                    }
                    finally
                    {
                        jobManager.RemoveJob(job.PaymentId);
                    }
                }
                catch (OperationCanceledException)
                {
                    // graceful shutdown
                    logger.LogInformation("PaymentStatusWorker stopping");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing payment job");
                }
            }
        }

        private async Task ProcessPayment(PaymentStatusJob job, CancellationToken token)
        {
            logger.LogInformation("Processing payment {PaymentId}", job.PaymentId);

            using var scope = scopeFactory.CreateScope();
            var paymentService = scope.ServiceProvider
                .GetRequiredService<IPaymentService>();

            const int maxAttempts = 60; // 5 min polling
            int attempt = 0;

            while (!token.IsCancellationRequested && attempt < maxAttempts)
            {
                attempt++;
                try
                {
                    var result = await paymentService.CheckStatusAsync(job.PaymentId, token);

                    if (result.Status != PaymemtStatusEnum.Pending)
                    {
                        logger.LogInformation(
                            "Payment {PaymentId} finished with status {Status}",
                            job.PaymentId,
                            result.Status);

                        return;
                    }
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("Payment {PaymentId} polling cancelled manually or worker stopped", job.PaymentId);
                    return;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error checking status for payment {PaymentId}", job.PaymentId);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), token);
            }

            if (attempt >= maxAttempts)
            {
                logger.LogWarning(
                    "Payment {PaymentId} polling stopped after max attempts",
                    job.PaymentId);
            }
        }
    }
}
