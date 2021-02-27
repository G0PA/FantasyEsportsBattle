using System;
using System.IO;
using System.Linq;
using System.Threading;
using FantasyEsportsBattle.InfoTracker.Sites;
using FantasyEsportsBattle.Web.Data;
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

            //var defaultImg = context.Images.FirstOrDefault(i => i.Id == Constants.DefaultImageId);
            //defaultImg.ImageData = File.ReadAllBytes("default.png");
            //context.SaveChanges();

            StartTrackers(context);
        }

        private static void StartTrackers(ApplicationDbContext dbContext)
        {
            GamesOfLegends gol = new GamesOfLegends();

            gol.ParseWebsiteOnInterval(dbContext, _workerBreakTime);
        }

    }
}
