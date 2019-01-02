namespace Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class Player
    {
        [JsonProperty("playerName"), Required]
        public string PlayerName { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("lastPlayed")]
        public string LastPlayed { get; set; }

        [JsonProperty("ThisTurn")]
        public string ThisTurn { get; set; }
    }
}
