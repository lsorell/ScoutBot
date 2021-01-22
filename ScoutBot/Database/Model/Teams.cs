using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoutBot.Database.Model
{
    public class Teams
    {
        /// <summary>
        /// The primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        /// <summary>
        /// The name of the team.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Forign key to the google sheet id.
        /// </summary>
        [ForeignKey("Sheets")]
        public int SheetId { get; set; }
        public Sheets Sheet { get; set; }
    }
}