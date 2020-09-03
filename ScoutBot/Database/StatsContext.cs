using Microsoft.EntityFrameworkCore;
using ScoutBot.Database.Model;

namespace ScoutBot.Services
{
    /// <summary>
    /// Context for the database.
    /// </summary>
    public class StatsContext : DbContext
    {
        //Datbase tables
        public DbSet<Provider> Providers { get; set; }

        /// <summary>
        /// Configures database to use sqlite
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@"Data Source=..\StatBot.db");
        }
    }
}