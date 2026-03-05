using System.Threading.Channels;

namespace smakchet.application.Helpers
{
    public interface IBackgroundQueueService<T>
    {
        Task Enqueue(T item);
        Task<T> DequeueAsync(CancellationToken cancellationToken);
    }


    public class BackgroundQueueService<T> : IBackgroundQueueService<T>
    {
        private readonly Channel<T> _queue;

        public BackgroundQueueService(int capacity = 3000)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = false,
                SingleWriter = false
            };

            _queue = Channel.CreateBounded<T>(options);
        }

        public async Task Enqueue(T item)
        {
            await _queue.Writer.WriteAsync(item);
        }

        public async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }


    public record PaymentStatusJob(int PaymentId);
}
