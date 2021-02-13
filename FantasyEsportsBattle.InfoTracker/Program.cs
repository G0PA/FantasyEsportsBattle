using System;
using System.IO;
using System.Threading;
using FantasyEsportsBattle.InfoTracker.Sites;
using FantasyEsportsBattle.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FantasyEsportsBattle.InfoTracker
{
    class Program
    {
        private static readonly TimeSpan _workerBreakTime = TimeSpan.FromMinutes(30);
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
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

    }
}
