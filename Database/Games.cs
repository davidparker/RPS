namespace Database
{
    using Constants;
    using Interfaces;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class Games : IDatabase<Game>
    {
        public async Task Create(Game data)
        {
            throw new NotImplementedException();
        }

        public Task<Game> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Game> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Game>> GetAll()
        {
            // use task.run because we are not async for this demo
           return await Task.Run(() => LoadGames());
        }

        /// <summary>
        /// Allow to query the data for any number of params
        /// </summary>
        /// <param name="predicate">A linq query like x=> x.Name == bob</param>
        /// <returns>List of objects</returns>
        public async Task<List<Game>> Query(Expression<Func<Game, bool>> predicate)
        {
           // LoadGames just returns a list, so here we cast as queryable so we can use our linq predicate
           var data = (IQueryable<Game>)(await Task.Run(() => LoadGames()));
           
           return data.Where(predicate).ToList();
        }

        public Task Update(Game data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Here to emulate loading from actual databse
        /// </summary>
        /// <returns></returns>
        private List<Game> LoadGames()
        {
            return new List<Game>
            {
                new Game
                {
                    Name = "Rock Paper Scissors",
                    NumberOfRounds = 3,
                    Pieces = new List<GamePiece>()
                    {
                        new GamePiece{ Name = "Rock", Beats = "Scissors", Icon = "fa fa-hand-rock" },
                        new GamePiece{ Name = "Paper", Beats = "Rock", Icon = "fa fa-hand-paper" },
                        new GamePiece{ Name = "Scissors", Beats = "Paper", Icon = "fa fa-hand-scissors" }
                    },
                    GameModes = RockPaperGameModes.GetAll()
                }
            };
        }
    }
}
