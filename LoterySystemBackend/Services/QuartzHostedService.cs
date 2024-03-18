using Quartz;
using Quartz.Spi;

namespace LoterySystemBackend.Services
{
    public class QuartzHostedService : BackgroundService
    {
        private readonly ISchedulerFactory _schdeulerFactory;
        private readonly IJobFactory _jobFactory;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _schdeulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scheduler = await _schdeulerFactory.GetScheduler(stoppingToken);
            scheduler.JobFactory = _jobFactory;

            await scheduler.Start(stoppingToken);
        }
    }
}
