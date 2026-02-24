namespace wot_api.DTO
{
    public class CompetionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RequestToJoin { get; set; }
        public string GameType { get; set; }
        public CompetionFormat Format { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxParticipants { get; set; }
        public string Location { get; set; }
        public List<string> Rules { get; set; }
        public List<ParticipantDTO> Participants { get; set; }
    }

    public enum CompetionFormat
    {
        Leaderboard,
        Tournament
    }
}
