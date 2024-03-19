using LoterySystemBackend.Controllers;
using LoterySystemBackend.Data;
using LoterySystemBackend.Interfaces;
using LoterySystemBackend.Job;
using LoterySystemBackend.Models;
using LoterySystemBackend.Repository;
using LoterySystemBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Spi;

namespace LotterySystemBackendTest
{
    public class LotteryControllerTest
    {
        private readonly LotteryDbContext _context;
        private readonly LotteryRepository _repository;
        private readonly LotteryController _controller;
        private readonly LotteryService _lotteryService;
        private readonly ServiceProvider _serviceProvider;
        private readonly ILoggerFactory _factory;

        public LotteryControllerTest()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddQuartz()
                .BuildServiceProvider();

            _factory = _serviceProvider.GetService<ILoggerFactory>();
            

            var options = new DbContextOptionsBuilder<LotteryDbContext>()
                .UseNpgsql("server=localhost;port=55432;database=testdb;User Id=postgres;Password=P@ssw0rd;")
                .Options;

            _context = new LotteryDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();

            var resposiotry_logger = _factory.CreateLogger<LotteryRepository>();
            _repository = new LotteryRepository(resposiotry_logger, _context);

            var lottery_logger = _factory.CreateLogger<LotteryService>();
            var myConfiguration = new OptionsWrapper<LotteryConfig>(new LotteryConfig
            {
                LotteryMaxLimit = 999999
            });
            _lotteryService = new LotteryService(lottery_logger, myConfiguration, _repository);

            var controllerlogger = _factory.CreateLogger<LotteryController>();
            _controller = new LotteryController(controllerlogger, _lotteryService);
        }

        [Fact]
        public async Task GenerateLotteryTest()
        {
            var OkResult = await _controller.PostLottery();
            Assert.NotNull(OkResult);
        }

        [Fact]
        public async Task DrawLotteryTest()
        {
            List<Lottery> LotteryList = new List<Lottery>();
            for (int i = 0; i < 50; i++)
            {
                var actionResult = await _controller.PostLottery();
                var lotteryResult = actionResult.Result as OkObjectResult;
                if (lotteryResult.Value != null)
                {
                    LotteryList.Add((Lottery)lotteryResult.Value);
                }
            }

            var drawJob_logger = _factory.CreateLogger<DrawLotteryJob>();
            var drawJob = new DrawLotteryJob(drawJob_logger, _lotteryService);
            await drawJob.Execute(null);

            var drawActionResult = _controller.GetLotteryResult();
            Winner DrawResult = (Winner)(drawActionResult.Result as OkObjectResult).Value;

            Assert.Contains(LotteryList, lottery => lottery.TicketNo == DrawResult.TicketNo);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}