using System.Text.Json.Serialization;

namespace Helium.Model
{
    public class FeaturedMovie
    {
        [JsonPropertyName("movieId")]
        public string MovieId { get; set; }
        [JsonPropertyName("weight")]
        public int Weight { get; set; } = 1;
    }
}
