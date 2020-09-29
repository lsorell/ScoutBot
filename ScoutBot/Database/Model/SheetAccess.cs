using System.ComponentModel.DataAnnotations;

namespace ScoutBot.Database.Model
{
    /// <summary>
    /// Used to track what discord roles can access which google sheets.
    /// </summary>
    public class SheetAccess
    {
        /// <summary>
        /// The google sheet id.
        /// </summary>
        public string SheetId { get; set; }

        /// <summary>
        /// The discord role id.
        /// </summary>
        public string RoleId { get; set; }
    }
}