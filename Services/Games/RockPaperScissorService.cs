namespace Services.Games
{
    using Interfaces;
    using Interfaces.Games;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RockPaperScissorService : IGame
    {
        private readonly IDatabase<Game> _GamesDatabase;

        // be a better way of doing this, but for demo this works
        private const int GamesId = 1;

        public RockPaperScissorService(IDatabase<Game> gamesdb) {
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
                return SetLastMoves(game, player1, player2);
            }

            // if player1 choice beats option == player2.thisTurn.   player 1 wins
            if (currentGame.Pieces.FirstOrDefault(x=> x.Name == player1.ThisTurn).Beats == player2.ThisTurn)
            {
                player1.Score++;
                game.RoundWinner = player1.PlayerName;
            } else
            {
                player2.Score++;
                game.RoundWinner = player2.PlayerName;
            } 

            return SetLastMoves(game, player1, player2);
        }

        public async Task<bool> IsCurrentGameAsync(string name)
        {
            return string.Equals((await _GamesDatabase.Get(GamesId)).Name, name);
        }

        public async Task<CurrentGame> MakeTurn(CurrentGame game)
        {
            if (game.Round == game.TotalRounds) throw new Exception($"Number of rounds is limited to {game.TotalRounds}.  PLease start another game.");
            game.Round++;
            game = await CalculateScore(game);
            return game;
        }

        private CurrentGame SetLastMoves(CurrentGame game, Player player1, Player player2)
        {

            player1.LastPlayed = player1.ThisTurn;
            player1.ThisTurn = null;

            player2.LastPlayed = player2.ThisTurn;
            player2.ThisTurn = null;

            game.Players = new List<Player>()
            {
                player1, player2
            };

            return game;
        }
    }
}
