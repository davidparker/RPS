using Interfaces.Games;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Games
{
    public class RockPaperScissorService : IGame
    {
        private readonly Database.Games _GamesDatabase;

        public RockPaperScissorService(Database.Games gamesdb) {
            _GamesDatabase = gamesdb;
        }

        public async Task<CurrentGame> CalculateScore(CurrentGame game)
        {
            var currentGame = (await _GamesDatabase.Query(x => x.Name == game.Name)).FirstOrDefault();

            if (currentGame == null) throw new Exception("Current game is invalid");

            var player1 = game.Players.First();
            var player2 = game.Players.Last();

            // Catch its draw
            if(player1.ThisTurn == player2.ThisTurn)
            {
                game.RoundWinner = "DRAW";
                return game;
            }

            // if player1 choice beats option == player2.thisTurn.   player 1 wins
            if (currentGame.Pieces.FirstOrDefault(x=> x.Name == player1.ThisTurn).Beats == player2.ThisTurn)
            {
                game.Players.First().Score++;
                game.RoundWinner = game.Players.First().PlayerName;
            } else
            {
                game.Players.Last().Score++;
                game.RoundWinner = game.Players.Last().PlayerName;
            } 

            return game;
        }

        public async Task<CurrentGame> MakeTurn(CurrentGame game)
        {
            if (game.Round == game.TotalRounds) throw new Exception($"Number of rounds is limited to {game.TotalRounds}.  PLease start another game.");
            game.Round++;
            game = await CalculateScore(game);
            return game;

        }
    }
}
