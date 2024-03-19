using LoterySystemBackend.Job;
using LoterySystemBackend.Models;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Spi;

namespace LoterySystemBackend.Services
{
    public class QuartzHostedService : BackgroundService
    {
        private readonly ISchedulerFactory _schdeulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly QuartzConfig _configuration;
        private IScheduler _scheduler;
        private ITrigger _drawLotteryJobTrigger;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IOptions<QuartzConfig> options)
        {
            _schdeulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _configuration = options.Value;
        }

        public DateTime GetNextTriggerTime()
        {
            return _drawLotteryJobTrigger.GetNextFireTimeUtc().Value.DateTime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _scheduler = await _schdeulerFactory.GetScheduler(stoppingToken);
            _scheduler.JobFactory = _jobFactory;
            await SetUpDrawLotteryJob();

            await _scheduler.Start(stoppingToken);
        }

        private async Task SetUpDrawLotteryJob()
        {
            string jobName = typeof(DrawLotteryJob).Name;
            var cronSchedule = _configuration.Quartz.DrawLotteryJob;
            var jobKey = new JobKey(jobName);
            IJobDetail drawLotteryJob = JobBuilder
                .Create<DrawLotteryJob>()
                .WithIdentity(jobKey.Name)
                .Build();
            _drawLotteryJobTrigger = TriggerBuilder
                .Create()
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule)
                .Build();
            await _scheduler.ScheduleJob(drawLotteryJob, _drawLotteryJobTrigger);
        }
    }
}
