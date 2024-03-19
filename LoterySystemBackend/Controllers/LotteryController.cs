using LoterySystemBackend.Interfaces;
using LoterySystemBackend.Models;
using LoterySystemBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoterySystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotteryController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly LotteryService _lotteryService;

        public LotteryController(ILogger<LotteryController> logger, ILotteryService lotteryService)
        {
            _logger = logger;
            _lotteryService = (LotteryService)lotteryService;
        }

        [HttpGet]
        [Route("/get_lottery_result")]
        public ActionResult<Winner> GetLotteryResult()
        {
            var last_winner = _lotteryService.GetLastWinner();
            if(last_winner == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(last_winner);
            }
        }

        [HttpGet]
        [Route("/get_next_draw_time")]
        public IActionResult GetNextDrawTime()
        {
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Lottery>> PostLottery()
        {
            var lottery = await _lotteryService.GenerateLottery();
            return Ok(lottery);
        }
    }
}
