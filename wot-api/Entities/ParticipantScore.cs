using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wot_api.Entities
{
    public class ParticipantScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Match")]
        public int MatchId { get; set; }
        [ForeignKey("Participant")]
        public int ParticipantId { get; set; }
        public int Score { get; set; }
        public virtual Match Match { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
