using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using smakchet.dal.Models;

namespace smakchet.application.Helpers
{
    internal class KHQRPaymentService(IServiceScopeFactory scopeFactory, ILogger<Payment> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (AppQueue.Queue.TryDequeue(out var task))
                {
                    logger.LogInformation($"[Worker] Executing background start");

                    try
                    {
                        using var scope = scopeFactory.CreateScope();
                        await task.ExecuteAsync(scope.ServiceProvider, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Worker] Error executing task: {ex.Message}");
                    }

                    logger.LogInformation($"[Worker] Executing background finished");
                }
                else
                {
                    await Task.Delay(300, stoppingToken);
                }
            }
        }
    }
}
