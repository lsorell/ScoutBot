using System.ComponentModel.DataAnnotations;

namespace ScoutBot.Database.Model
{
    /// <summary>
    /// Table containing google sheet data.
    /// </summary>
    public class Sheets
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        [Key]
        public int SheetId { get; set; }

        /// <summary>
        /// The google sheet id.
        /// </summary>
        public string GoogleId { get; set; }
    }
}