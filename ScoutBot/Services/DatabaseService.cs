
using ScoutBot.Database.Model;
using System.Collections.Generic;
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
        public static async Task<bool> AddSheetAsync(string googleId, string name)
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

        public static async Task<bool> AddSheetAccessAsync(ulong[] roles, string name)
        {
            using (ScoutContext db = new ScoutContext())
            {
                try
                {
                    Sheets sheet = await db.Sheets
                                        .AsAsyncEnumerable()
                                        .Where(s => s.Name == name)
                                        .FirstAsync<Sheets>();

                    foreach (ulong role in roles)
                    {
                        db.SheetAccess.Add(new SheetAccess
                        {
                            RoleId = role,
                            SheetId = sheet.SheetId
                        });
                    }
                    await db.SaveChangesAsync();
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets all the common sheet names from the sheets table.
        /// </summary>
        /// <returns>A list of all sheet names.</returns>
        public static async Task<List<string>> GetSheetNamesAsync()
        {
            using (ScoutContext db = new ScoutContext())
            {
                return await db.Sheets
                    .AsAsyncEnumerable()
                    .Select(s => s.Name)
                    .ToListAsync<string>();
            }
        }
    }
}