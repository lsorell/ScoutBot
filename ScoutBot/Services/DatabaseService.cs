using Microsoft.EntityFrameworkCore;
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
                catch (DbUpdateException)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds a sheet access object to the database.
        /// </summary>
        /// <param name="roles">The roleId's to add.</param>
        /// <param name="name">The name of the sheet to give access to.</param>
        /// <returns>Bool reflecting if the changes went through.</returns>
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
                catch (DbUpdateException)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets all the common sheet names from the Sheets table.
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

        /// <summary>
        /// Gets a list of all RoleIds in the SheetAccess table.
        /// </summary>
        /// <returns>A list of all roles.</returns>
        public static async Task<List<SheetAccess>> GetAccessibleSheetsAsync(List<ulong> roles)
        {
            List<SheetAccess> sheets = new List<SheetAccess>();
            using (ScoutContext db = new ScoutContext())
            {
                foreach (ulong role in roles)
                {
                    sheets.AddRange(await db.SheetAccess
                        .Include(sa => sa.Sheet)
                        .AsAsyncEnumerable()
                        .Where(sa => sa.RoleId == role)
                        .ToListAsync<SheetAccess>());
                }
            }
            // Remove duplicate sheetIds
            HashSet<int> seen = new HashSet<int>();
            foreach (SheetAccess sa in sheets.ToList())
            {
                if (seen.Contains(sa.SheetId))
                {
                    sheets.Remove(sa);
                }
                else
                {
                    seen.Add(sa.SheetId);
                }
            }
            return sheets;
        }
    }
}