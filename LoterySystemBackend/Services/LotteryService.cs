using LoterySystemBackend.Data;
using LoterySystemBackend.Interfaces;
using LoterySystemBackend.Models;
using LoterySystemBackend.Repository;
using Microsoft.Extensions.Options;

namespace LoterySystemBackend.Services
{
    public class LotteryService : ILotteryService
    {
        private readonly ILogger _logger;
        private readonly LotteryRepository _lotteryRepository;
        private readonly IOptions<Configuration> _configuration;

        public LotteryService(ILogger<LotteryService> logger, IOptions<Configuration> configuration, LotteryRepository lotteryRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _lotteryRepository = lotteryRepository;
        }

        public async Task DrawLottery()
        {
            _logger.LogInformation("Begin Draw Lottery");
            Draw draw = new Draw(DateTime.Now);
            List<Lottery> lotteryPool = this._lotteryRepository.GetAvailableLotteriesByDatetime(draw.DrawDatetime);
            if (lotteryPool.Count == 0)
            {
                _logger.LogInformation("No Lottery is found. No Draw will be processed");
            }
            else
            {
                draw = await this._lotteryRepository.CreateDrawAsync(draw);

                Random random = new Random();
                int num = random.Next(lotteryPool.Count);

                var winner = lotteryPool[num];
                _logger.LogInformation("Winner is {0}", winner.TicketNo);
                draw.WinnerTicketNo = winner.TicketNo;
                await this._lotteryRepository.UpdateDrawAsync(draw);
                lotteryPool.ForEach(lottery => lottery.Discarded = true);
                await this._lotteryRepository.UpdateLotteryRangeAsync(lotteryPool);
            }
            _logger.LogInformation("End Draw Lottery");
        }

        public async Task<Lottery> GenerateLottery()
        {
            bool exist = false;
            string ticketNo;
            int lotteryMax = _configuration.Value.LotteryMaxLimit;
            var tickets = _lotteryRepository.GetAllAvailableTicketNo();
            do
            {
                Random random = new Random();
                int num = random.Next(lotteryMax);
                ticketNo = num.ToString().PadLeft(lotteryMax.ToString().Length, '0');
                exist = tickets.Any(ticket => ticket == ticketNo);
            }
            while (exist && tickets.Count < lotteryMax);

            Lottery lottery = new Lottery(ticketNo);
            lottery = await _lotteryRepository.CreateLotteryAsync(lottery);

            _logger.LogInformation("TicketNo :{0} is generated", lottery.TicketNo);

            return lottery;
        }

        public Winner GetLastWinner()
        {
            var ticketNo = _lotteryRepository.GetLatestDraw().WinnerTicketNo;
            if (ticketNo == null)
            {
                return null;
            }
            else
            {
                Winner winner = new Winner(ticketNo);
                return winner;
            }
        }
    }
}
