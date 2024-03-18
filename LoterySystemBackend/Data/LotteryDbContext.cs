using LoterySystemBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace LoterySystemBackend.Data
{
    public class LotteryDbContext :DbContext
    {
        public LotteryDbContext(DbContextOptions<LotteryDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Lottery> Lottery { get; set; } = default!;
        public DbSet<Draw> Draw { get; set; } = default!;
    }
}
