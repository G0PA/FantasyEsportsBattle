using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using FantasyEsportsBattle.Host.Data.Models;
using FantasyEsportsBattle.Host.Data.Models.Tournament;

namespace FantasyEsportsBattle.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }
        public DbSet<TournamentPlayerStats> TournamentPlayerStatuses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
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
        }
    }
}
