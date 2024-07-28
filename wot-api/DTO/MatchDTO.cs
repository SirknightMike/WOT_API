namespace wot_api.DTO
{
    public class MatchDTO
    {
        public int Id { get; set; }
        public int CompetionId { get; set; }
        public List<ParticipantDTO> Participants { get; set; }
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }
        public string Result { get; set; }
        public int ScoreParticipant {  get; set; }
        public int Location { get; set; }
    }

    public enum MatchStatus
    {
        Scheduled,
        Ongoing,
        Completed
    }

    
}
