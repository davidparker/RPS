namespace Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        [JsonProperty("name"), Required, StringLength(200)]
        public string Name { get; set; }

        [JsonProperty("numberOfRounds"), Required, MinLength(1)]
        public int NumberOfRounds { get; set; }

        [JsonProperty("pieces")]
        public ICollection<GamePiece> Pieces { get; set; }

        [JsonProperty("modes")]
        public List<string> GameModes { get; set; }
    }
}
