using LoterySystemBackend.Controllers;
using LoterySystemBackend.Data;
using LoterySystemBackend.Models;

namespace LoterySystemBackend.Repository
{
    public class LotteryRepository
    {
        private ILogger<LotteryRepository> _logger;
        private LotteryDbContext _lotteryDbContext;

        public LotteryRepository(ILogger<LotteryRepository> logger, LotteryDbContext context) 
        {
            _logger = logger;
            _lotteryDbContext = context;
        }

        public List<Lottery> GetAvailableLotteriesByDatetime(DateTime datetime)
        {
            var lotteries = (from l in _lotteryDbContext.Lottery
                           where l.IssueDateTime <= datetime && l.Discarded == false
                           select l).ToList();

            return lotteries;
        }

        public List<string> GetAllAvailableTicketNo()
        {
            var tickets = (from l in _lotteryDbContext.Lottery
                     where l.Discarded == false
                     select l.TicketNo).ToList();

            return tickets;
        }

        public Draw GetLatestDraw()
        {
            var draw = (from d in _lotteryDbContext.Draw
                     orderby d.DrawDatetime descending
                     select d).Take(1).ToList().First();
            return draw;
        }


        public async Task<Lottery> CreateLotteryAsync(Lottery lottery)
        {
            await _lotteryDbContext.Lottery.AddAsync(lottery);
            await _lotteryDbContext.SaveChangesAsync();

            return lottery;
        }

        public async Task<List<Lottery>> UpdateLotteryRangeAsync(List<Lottery> lotteries)
        {
            _lotteryDbContext.Lottery.UpdateRange(lotteries);
            await _lotteryDbContext.SaveChangesAsync();

            return lotteries;
        }

        public async Task<Draw> CreateDrawAsync(Draw draw)
        {
            await _lotteryDbContext.Draw.AddAsync(draw);
            await _lotteryDbContext.SaveChangesAsync();

            return draw;
        }

        public async Task<Draw> UpdateDrawAsync(Draw draw)
        {
            _lotteryDbContext.Draw.Update(draw);
            await _lotteryDbContext.SaveChangesAsync();

            return draw;
        }
    }
}
