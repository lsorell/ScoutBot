using Microsoft.EntityFrameworkCore;
using RiotSharp.Endpoints.MatchEndpoint;
using ScoutBot.Database.Model;

namespace ScoutBot.Services
{
    /// <summary>
    /// Context for the database.
    /// </summary>
    public class ScoutContext : DbContext
    {
        /// <summary>
        /// The database tables.
        /// </summary>
        public DbSet<SheetAccess> SheetAccess { get; set; }
        public DbSet<Sheets> Sheets { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Matches> Matches { get; set; }

        /// <summary>
        /// Configures database to use sqlite.
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@"Data Source=..\ScoutBot.db");
        }

        /// <summary>
        /// Adds constraints to tables.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ScoutBot Tables
            builder.Entity<Sheets>()
                .HasIndex(s => s.GoogleId)
                .IsUnique(true);

            builder.Entity<Sheets>()
                .HasIndex(s => s.Name)
                .IsUnique(true);

            builder.Entity<SheetAccess>()
                .HasIndex(sa => new { sa.RoleId, sa.SheetId })
                .IsUnique(true);

            builder.Entity<Teams>()
                .HasIndex(t => new { t.Name, t.SheetId })
                .IsUnique(true);

            // Riot Api Tables
            builder.Ignore<Mastery>();
            builder.Ignore<Rune>();

            // Ignore these dictionaries, ef does not map them.
            builder.Entity<ParticipantTimeline>()
                .Ignore(pt => pt.GoldPerMinDeltas);
            builder.Entity<ParticipantTimeline>()
                .Ignore(pt => pt.XpDiffPerMinDeltas);
            builder.Entity<ParticipantTimeline>()
                .Ignore(pt => pt.XpPerMinDeltas);
            builder.Entity<ParticipantTimeline>()
                .Ignore(pt => pt.CsDiffPerMinDeltas);
            builder.Entity<ParticipantTimeline>()
                .Ignore(pt => pt.CreepsPerMinDeltas);
            builder.Entity<ParticipantTimeline>()
                .Ignore(pt => pt.DamageTakenDiffPerMinDeltas);
            builder.Entity<ParticipantTimeline>()
                .Ignore(pt => pt.DamageTakenPerMinDeltas);
        }
    }
}