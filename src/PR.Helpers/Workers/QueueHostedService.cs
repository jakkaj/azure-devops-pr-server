using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PR.Helpers.Contract;

namespace PR.Helpers.Workers
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogService _logger;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue,
            ILogService logger)
        {
            TaskQueue = taskQueue;
            _logger = logger;
        }

        public IBackgroundTaskQueue TaskQueue { get; }

        protected async override Task ExecuteAsync(
            CancellationToken cancellationToken)
        {
            _logger.TrackTrace("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(cancellationToken);

                try
                {
                    await workItem(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.TrackException(ex,
                        $"Error occurred executing {nameof(workItem)}.");
                }
            }

            _logger.TrackTrace("Queued Hosted Service is stopping.");
        }
    }
}
