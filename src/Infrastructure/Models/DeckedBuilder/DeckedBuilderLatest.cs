using System.Text.Json.Serialization;

namespace Infrastructure.Models.DeckedBuilder {
    public class DeckedBuilderLatest {
        [JsonPropertyName("version")]
        public int Version { get; set; } 

        [JsonPropertyName("description")]
        public string Description { get; set; } 

        [JsonPropertyName("source")]
        public string Source { get; set; } 

        [JsonPropertyName("md5")]
        public string Md5 { get; set; } 
    }
}
