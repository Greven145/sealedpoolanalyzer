using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infrastructure.Models.tmp.Scryfall {
    public class SearchResult {
        [JsonPropertyName("object")] public string Object { get; set; }

        [JsonPropertyName("total_cards")] public int TotalCards { get; set; }

        [JsonPropertyName("has_more")] public bool HasMore { get; set; }

        [JsonPropertyName("next_page")] public string NextPage { get; set; }

        [JsonPropertyName("data")] public List<Card> Data { get; set; }
    }
}
