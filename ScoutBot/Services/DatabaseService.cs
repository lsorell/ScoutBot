using ScoutBot.Database.Model;
using System.Linq;
using System.Threading.Tasks;

namespace ScoutBot.Services
{
    /// <summary>
    /// Handles database operations.
    /// </summary>
    public class DatabaseService
    {
        /// <summary>
        /// Adds the providerId to the database.
        /// </summary>
        /// <param name="providerId">The id from riot for generating tournaments.</param>
        public static async Task PutProviderAsync(int providerId)
        {
            using (StatsContext db = new StatsContext())
            {
                await db.Providers.AddAsync(new Provider { ProviderID = providerId });
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Sees if there is already an entry in the providers table.
        /// </summary>
        public static async Task<bool> ProviderExistsAsync()
        {
            using (StatsContext db = new StatsContext())
            {
                return await db.Providers.AnyAsync();
            }
        }
    }
}