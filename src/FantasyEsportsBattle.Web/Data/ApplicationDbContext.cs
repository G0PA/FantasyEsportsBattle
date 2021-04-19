using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FantasyEsportsBattle.Models;
using FantasyEsportsBattle.Models.Tournament;
namespace FantasyEsportsBattle.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<CompetitionPlayer> CompetitionPlayers { get; set; }
        public DbSet<CompetitionPlayerStats> CompetitionPlayerStatuses { get; set; }
        public DbSet<ApplicationUserTournament> ApplicationUserTournaments { get; set; }
        public DbSet<TournamentCompetition> TournamentCompetitions { get; set; }
        public DbSet<FinishedEvent> FinishedEvents { get; set; }
        public DbSet<TournamentInvitation> TournamentInvitations { get; set; }
        public DbSet<TournamentStats> TournamentStatuses { get; set; }
        public DbSet<TournamentBoughtPlayer> TournamentBoughtPlayers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CompetitionPlayer>()
                .HasOne(cp => cp.CompetitionPlayerStats)
                .WithOne(p => p.CompetitionPlayer)
                .HasForeignKey<CompetitionPlayerStats>(cp => cp.CompetitionPlayerId);

            builder.Entity<CompetitionPlayer>()
                .HasOne(t => t.Team)
                .WithMany(t => t.Players);

            builder.Entity<Team>()
                .HasOne(t => t.Competition)
                .WithMany(c => c.Teams);

            builder.Entity<ApplicationUserTournament>().HasKey(appUserTournament =>
                new {appUserTournament.TournamentId, appUserTournament.ApplicationUserId});

            builder.Entity<ApplicationUserTournament>()
                .HasOne(aut => aut.Tournament)
                .WithMany(t => t.ApplicationUserTournaments)
                .HasForeignKey(aut => aut.TournamentId);

            builder.Entity<ApplicationUserTournament>()
                .HasOne(aut => aut.ApplicationUser)
                .WithMany(t => t.ApplicationUserTournaments)
                .HasForeignKey(aut => aut.ApplicationUserId);


            builder.Entity<TournamentCompetition>().HasKey(tourCompetition =>
                new { tourCompetition.TournamentId, tourCompetition.CompetitionId });

            builder.Entity<TournamentCompetition>()
                .HasOne(tourCompetition => tourCompetition.Tournament)
                .WithMany(t => t.TournamentCompetitions)
                .HasForeignKey(aut => aut.TournamentId);

            builder.Entity<TournamentCompetition>()
                .HasOne(tourCompetition => tourCompetition.Competition)
                .WithMany(t => t.TournamentCompetitions)
                .HasForeignKey(aut => aut.CompetitionId);

            builder.Entity<Tournament>()
                .HasOne(t => t.TournamentHost)
                .WithMany(u => u.HostedTournaments)
                .HasForeignKey(t => t.TournamentHostId);

            builder.Entity<TournamentInvitation>()
                .HasOne(ti => ti.InvitedUser)
                .WithMany(u => u.TournamentInvitations);

            builder.Entity<TournamentBoughtPlayer>()
                .HasOne(t => t.TournamentStats)
                .WithMany(ts => ts.BoughtPlayers)
                .HasForeignKey(t => t.TournamentStatsId);
        }
    }
}
