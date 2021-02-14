using System.Text.Json.Serialization;

namespace Infrastructure.Models.tmp.Scryfall {
    public class Preview {
        [JsonPropertyName("source")] public string Source { get; set; }

        [JsonPropertyName("source_uri")] public string SourceUri { get; set; }

        [JsonPropertyName("previewed_at")] public string PreviewedAt { get; set; }
    }
}
