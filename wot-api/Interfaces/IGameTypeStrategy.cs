using wot_api.Entities;

namespace wot_api.Interfaces
{
    public interface IGameTypeStrategy
    {
        int CalculatePoints(Match match);
        int MaxPlayers { get; }
    }
}
