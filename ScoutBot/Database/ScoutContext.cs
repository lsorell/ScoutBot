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

        /// <summary>
        /// Configures database to use sqlite
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@"Data Source=..\ScoutBot.db");
        }

        /// <summary>
        /// Sets attributes of database when model is being created.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SheetAccess>().HasKey(s => new
            {
                s.SheetId,
                s.RoleId
            });
        }
    }
}