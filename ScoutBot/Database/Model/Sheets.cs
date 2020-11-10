using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SheetId { get; set; }

        /// <summary>
        /// The google sheet id.
        /// </summary>
        public string GoogleId { get; set; }

        /// <summary>
        /// The common name for the sheet.
        /// </summary>
        public string Name { get; set; }
    }
}