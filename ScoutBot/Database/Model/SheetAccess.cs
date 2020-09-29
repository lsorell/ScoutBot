using System.ComponentModel.DataAnnotations;

namespace ScoutBot.Database.Model
{
    /// <summary>
    /// Used to track what discord roles can access which google sheets.
    /// </summary>
    public class SheetAccess
    {
        /// <summary>
        /// The discord role id.
        /// </summary>
        [Key]
        public ulong RoleId { get; set; }

        /// <summary>
        /// The google sheet id.
        /// </summary>
        public string SheetId { get; set; }

        /// <summary>
        /// The name of the team that will be using the scout sheet.
        /// </summary>
        public string TeamName { get; set; }
    }
}