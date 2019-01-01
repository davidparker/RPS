namespace Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class GamePiece
    {
        [JsonProperty("name"), Required, StringLength(200)]
        public string Name { get; set; }

        [JsonProperty("beats")]
        public string Beats { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
