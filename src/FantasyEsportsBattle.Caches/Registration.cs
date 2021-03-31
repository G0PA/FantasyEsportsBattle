using FantasyEsportsBattle.Caches.Interface;
using FantasyEsportsBattle.Models.Tournament;
using Microsoft.Extensions.DependencyInjection;

namespace FantasyEsportsBattle.Caches
{
    public static class Registration
    {
        public static IServiceCollection RegisterCompetitionsCache(this IServiceCollection services)
        {
            services.AddSingleton<CompetitionsCache>();

            return services;
        }
    }
}
