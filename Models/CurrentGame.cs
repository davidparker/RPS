namespace Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class CurrentGame
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("Players")]
        public ICollection<Player> Players { get; set; }

        [JsonProperty("round")]
        public int Round { get; set; }

        [JsonProperty("totalRounds")]
        public int TotalRounds { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("roundWinner")]
        public string RoundWinner { get; set; }
    }
}
