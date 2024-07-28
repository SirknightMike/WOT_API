using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using wot_api.DTO;

namespace wot_api.Entities
{
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Competition")]
        public int CompetitionId { get; set; }
        public List<Participant> Participants { get; set; }
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }
        public string Result { get; set; }
        public string Location { get; set; }
        public Competition Competition { get; set; }
        public List<ParticipantScore> ParticipantScores { get; set; }
    }

    public enum MatchStatus
    {
        Scheduled,
        Ongoing,
        Completed
    }
}
