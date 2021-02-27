using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using FantasyEsportsBattle.Host.Data.Models;
using FantasyEsportsBattle.Web.Data.Models;
using FantasyEsportsBattle.Web.Data.Models.Tournament;

namespace FantasyEsportsBattle.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }
        public DbSet<TournamentPlayerStats> TournamentPlayerStatuses { get; set; }
        public DbSet<ApplicationUserTournament> ApplicationUserTournaments { get; set; }
        public DbSet<TournamentCompetition> TournamentCompetitions { get; set; }
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

            builder.Entity<TournamentPlayer>()
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
        }
    }
}
