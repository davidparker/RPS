namespace Services
{
    using Interfaces;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;

    public class GameInfoService
    {
        private readonly IDatabase<Game> _database;

        public GameInfoService(IDatabase<Game> database)
        {
            _database = database;
        }

        public async Task<List<Game>> GetAllGames()
        {
            return await _database.GetAll();
        }

        public async Task<CurrentGame> NewGame(int id)
        {
            var requestedGame = await _database.Get(id);

            if (requestedGame == null) throw new NullReferenceException("Game does not exist");

            return new CurrentGame
            {
                Name = requestedGame.Name,
                Mode = "",
                Players = new List<Player>(),
                Round = 1,
                TotalRounds = requestedGame.NumberOfRounds,
                RoundWinner = null
            };
        }
    }
}
