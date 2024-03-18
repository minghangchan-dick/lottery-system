using LoterySystemBackend.Interfaces;
using LoterySystemBackend.Services;
using Quartz;

namespace LoterySystemBackend.Job
{
    [DisallowConcurrentExecution]
    public class DrawLotteryJob : IJob
    {
        private readonly ILogger<DrawLotteryJob> _logger;
        private readonly LotteryService _lotteryService;

        public DrawLotteryJob(ILogger<DrawLotteryJob> logger, ILotteryService lotteryService) 
        {
            _logger = logger;
            _lotteryService = (LotteryService)lotteryService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _lotteryService.DrawLottery();
            }
            catch (Exception ex)
            {
                _logger.LogError("DrawLotteryJob Execute: {0}", ex);
            }
        }
    }
}
