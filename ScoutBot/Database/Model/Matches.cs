using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiotSharp.Endpoints.MatchEndpoint;

namespace ScoutBot.Database.Model
{
    public class Matches
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatchId { get; set; }

        /// <summary>
        /// The result (w/l) of the team being scouted.
        /// </summary>
        public char Result { get; set; }

        /// <summary>
        /// The team that is being scouted.
        /// </summary>
        [ForeignKey("Teams")]
        public int TeamId { get; set; }
        public Teams Team { get; set; }

        /// <summary>
        /// The match history details.
        /// </summary>
        public Match Match { get; set; }
    }
}