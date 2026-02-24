using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using wot_api.DTO;

namespace wot_api.Entities
{
    public class Competition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RequestTojoin { get; set; }
        [Required]
        public string GameType { get; set; }
        [Required]
        public CompetionFormat Format { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxParticipants { get; set; }
        public string Location { get; set; }
        public List<string> Rules { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Match> Matches { get; set; }
    }

    public enum CompetitionFormat
    {
        Leaderboard,
        Tournament
    }

}
