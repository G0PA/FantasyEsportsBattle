using System;
using System.IO;
using FantasyEsportsBattle.InfoTracker.Sites;
using FantasyEsportsBattle.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace FantasyEsportsBattle.InfoTracker
{
    class Program
    {
        private static readonly TimeSpan _workerBreakTime = TimeSpan.FromMinutes(30);
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.File(@"../../../log/logRegions.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            var context = new ApplicationDbContext(optionsBuilder.Options);
            StartTrackers(context);
        }

        private static void StartTrackers(ApplicationDbContext dbContext)
        {
            GamesOfLegends gol = new GamesOfLegends();

            gol.ParseWebsiteOnInterval(dbContext, _workerBreakTime);
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
