using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoutBot.Database.Model
{
    /// <summary>
    /// Used to track what discord roles can access which google sheets.
    /// </summary>
    public class SheetAccess
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SheetAccessId { get; set; }

        /// <summary>
        /// The discord role id.
        /// </summary>
        public ulong RoleId { get; set; }

        /// <summary>
        /// Forign key to the google sheet id.
        /// </summary>
        [ForeignKey("Sheets")]
        public int SheetId { get; set; }
        public Sheets Sheet { get; set; }
    }
}