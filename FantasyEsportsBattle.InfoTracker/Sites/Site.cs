using System;
using System.Net.Http;
using FantasyEsportsBattle.Data;

namespace FantasyEsportsBattle.InfoTracker.Sites
{
    public abstract class Site
    {
        protected HttpClient Client;
        public abstract void ParseWebsiteOnInterval(ApplicationDbContext dbContext, TimeSpan interval);

        public Site()
        {
            Client = new HttpClient();

            Client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36");
        }
    }
}
