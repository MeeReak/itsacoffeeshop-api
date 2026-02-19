using System.Collections.Concurrent;
using smakchet.application.Interfaces.IBackground;

namespace smakchet.application.Helpers
{
    public static class AppQueue
    {
        public static ConcurrentQueue<IBackgroundService> Queue = new();
    }
}
