using LoterySystemBackend.Models;

namespace LoterySystemBackend.Interfaces
{
    public interface ILotteryService
    {
        public Winner GetLastWinner();
        public Task<Lottery> GenerateLottery();
        public Task DrawLottery();
    }
}
