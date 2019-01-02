namespace Interfaces.Games
{
    using Models;
    using System.Threading.Tasks;

    public interface IGame
    {
        Task<CurrentGame> CalculateScore(CurrentGame game);
        Task<CurrentGame> MakeTurn(CurrentGame game);
        Task<bool> IsCurrentGameAsync(string name);
    }
}
