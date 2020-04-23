using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Helium.Model
{
    public class Movie
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("partitionKey")]
        public string PartitionKey { get; set; }
        [JsonPropertyName("movieId")]
        public string MovieId { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }
        [JsonPropertyName("rating")]
        public double Rating { get; set; }
        [JsonPropertyName("votes")]
        public long Votes { get; set; }
        [JsonPropertyName("totalScore")]
        public long TotalScore { get; set; }
        [JsonPropertyName("textSearch")]
        public string TextSearch { get; set; }
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }
        [JsonPropertyName("roles")]
        public List<Role> Roles { get; set; }
    }

    public class Role
    {
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("actorId")]
        public string ActorId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("birthYear")]
        public int? BirthYear { get; set; }
        [JsonPropertyName("deathYear")]
        public int? DeathYear { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("characters")]
        public List<string> Characters { get; set; }
    }
}
