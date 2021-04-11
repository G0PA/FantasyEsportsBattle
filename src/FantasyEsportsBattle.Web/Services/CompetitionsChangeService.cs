using FantasyEsportsBattle.Caches;
using FantasyEsportsBattle.Models.Tournament;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Services
{
    public class CompetitionsChangeService : IHostedService
    {
        private readonly CompetitionsCache _competitionsCache;

        public CompetitionsChangeService(CompetitionsCache competitionsCache)
        {
            _competitionsCache = competitionsCache;

            _competitionsCache.OnItemRemoved = HandleRemovedCompetition;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void HandleRemovedCompetition(Competition competition)
        {

        }
    }
}
