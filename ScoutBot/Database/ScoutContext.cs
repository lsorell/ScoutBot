using Microsoft.EntityFrameworkCore;
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
            builder.Entity<Sheets>()
                .HasIndex(s => s.GoogleId)
                .IsUnique(true);

            builder.Entity<Sheets>()
                .HasIndex(s => s.Name)
                .IsUnique(true);
        }
    }
}