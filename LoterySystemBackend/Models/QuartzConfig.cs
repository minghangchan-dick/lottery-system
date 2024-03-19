namespace LoterySystemBackend.Models
{
    public class QuartzConfig
    {
        public QuartzClass Quartz { get; set; }

        public class QuartzClass
        {
            public string DrawLotteryJob { get; set; }
        }
    }
}
