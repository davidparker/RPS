namespace API.Controllers
{
    using Interfaces.Games;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/Game")]
    [ApiController]
    [EnableCors("cors")]
    public class GameController : ControllerBase
    {
        private readonly GameInfoService _gameInfoService;

        // the list of all available games
        private readonly IEnumerable<IGame> _games;

        public GameController(GameInfoService gameInfoService, IEnumerable<IGame> games)
        {
            _gameInfoService = gameInfoService;
            _games = games;
        }

        //get games
        [HttpGet, Route("GetGames")]
        public async Task<IActionResult> GetGames()
        {
            return new JsonResult(await _gameInfoService.GetAllGames());
        }

        [HttpGet, Route("NewGame/{id}")]
        public async Task<IActionResult> NewGame(int id)
        {
            return new JsonResult(await _gameInfoService.NewGame(id));
        }


        [HttpPost, Route("MakeTurn")]
        public async Task<IActionResult> MakeTurn([FromBody] CurrentGame game)
        {
            // seems as games all follow inerface rules, we should be able to add any 
            // number of games we like, and then loop through the collection the find the game
            // currently being played.

            foreach (var g in _games)
            {
                if((await g.IsCurrentGameAsync(game.Name)))
                {
                    return new JsonResult(await g.MakeTurn(game));
                }
            }

            throw new System.Exception("Game is invalid");
        }
    }
}