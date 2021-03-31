using System;
using System.IO;
using System.Linq;
using FantasyEsportsBattle.InfoTracker.Sites;
using FantasyEsportsBattle.Web.Constants;
using FantasyEsportsBattle.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using FantasyEsportsBattle.Models;
using FantasyEsportsBattle.Caches;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FantasyEsportsBattle.InfoTracker
{
    class Program
    {
        private static readonly TimeSpan _workerBreakTime = TimeSpan.FromMinutes(30);
        static void Main(string[] args)
        {


            using IHost host = CreateHostBuilder(args).Build();

            var builder = new ConfigurationBuilder();

            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
       .ReadFrom.Configuration(builder.Build())
       .Enrich.FromLogContext()
       .WriteTo.File(@"log/logErrors.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 2000_0000, retainedFileCountLimit: 20)
       .CreateLogger();

            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var defaultImg = context.Images.FirstOrDefault(i => i.Id == Constants.DefaultImageId);
            if (defaultImg == null)
            {
                var img = new Image
                {
                    ImageTitle = "DefaultImage"
                };

                img.ImageData = File.ReadAllBytes("default.png");
                context.Images.Add(img);
                context.SaveChanges();
            }
            else if (defaultImg.ImageData == null)
            {

                defaultImg.ImageData = File.ReadAllBytes("default.png");
                context.SaveChanges();
            }

            StartTrackers(host.Services, context);
        }



        static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureServices((_, services) =>
                   services.RegisterCompetitionsCache());

        private static void StartTrackers(IServiceProvider services,ApplicationDbContext dbContext)
        {
            GamesOfLegends gol = new GamesOfLegends();

            var competitionsCache = (CompetitionsCache) services.GetService(typeof(CompetitionsCache));
            gol.ParseWebsiteOnInterval(dbContext, _workerBreakTime, competitionsCache);
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
