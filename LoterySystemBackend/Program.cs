using LoterySystemBackend.Data;
using LoterySystemBackend.Extensions;
using LoterySystemBackend.Interfaces;
using LoterySystemBackend.Job;
using LoterySystemBackend.Models;
using LoterySystemBackend.Repository;
using LoterySystemBackend.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace LoterySystemBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connection_string = Environment.GetEnvironmentVariable("LotteryDbContext") ?? builder.Configuration.GetConnectionString("LotteryDbContext") ?? throw new InvalidOperationException("Connection string 'LotteryDbContext' not found.");
            builder.Services.AddDbContext<LotteryDbContext>(options =>
                options.UseNpgsql(connection_string));

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();


            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            //builder.Services.AddQuartz(q =>
            //{
            //    q.AddJobAndTrigger<DrawLotteryJob>(builder.Configuration);
            //});
            builder.Services.Configure<LotteryConfig>(builder.Configuration);
            builder.Services.Configure<QuartzConfig>(builder.Configuration);

            builder.Services.AddQuartz();
            builder.Services.AddHostedService<QuartzHostedService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<LotteryRepository>();
            builder.Services.AddScoped<ILotteryService, LotteryService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MigrateDatabase<LotteryDbContext>();

            app.Run();
        }
    }
}