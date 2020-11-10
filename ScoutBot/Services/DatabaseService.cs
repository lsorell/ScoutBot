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
        /// Adds a sheet to the sheets table.
        /// </summary>
        /// <param name="googleId">The id of the google sheet.</param>
        /// <param name="name">The common name for the document.</param>
        /// <returns></returns>
        public static async Task<bool> AddSheet(string googleId, string name)
        {
            using (ScoutContext db = new ScoutContext())
            {
                try
                {
                    await db.Sheets.AddAsync(new Sheets
                    {
                        GoogleId = googleId,
                        Name = name
                    });
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}