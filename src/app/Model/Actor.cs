using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Helium.Model
{
    public class Actor
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("actorId")]
        public string ActorId { get; set; }
        [JsonPropertyName("partitionKey")]
        public string PartitionKey { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("birthYear")]
        public int? BirthYear { get; set; }
        [JsonPropertyName("deathYear")]
        public int? DeathYear { get; set; }
        [JsonPropertyName("textSearch")]
        public string TextSearch { get; set; }
        [JsonPropertyName("profession")]
        public List<string> Profession { get; set; }
        [JsonPropertyName("movies")]
        public List<ActorMovie> Movies { get; set; }
    }

    public class ActorMovie
    {
        [JsonPropertyName("movieId")]
        public string MovieId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }
    }
}
