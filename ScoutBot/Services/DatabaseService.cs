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
        /// Adds new SheetAccess row to the table.
        /// </summary>
        /// <param name="sheetId">The google sheet id.</param>
        /// <param name="roleId">The discord role id.</param>
        /// <returns></returns>
        public static async Task<bool> AddSheetAccess(string sheetId, ulong roleId, string teamName)
        {
            using (ScoutContext db = new ScoutContext())
            {
                try
                {
                    await db.SheetAccess.AddAsync(new SheetAccess
                    {
                        SheetId = sheetId,
                        RoleId = roleId,
                        TeamName = teamName
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