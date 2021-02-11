using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Logic.models.Scryfall
{
    public class CardFace
    {
        [JsonPropertyName("object")] public string Object { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("mana_cost")] public string ManaCost { get; set; }

        [JsonPropertyName("type_line")] public string TypeLine { get; set; }

        [JsonPropertyName("oracle_text")] public string OracleText { get; set; }

        [JsonPropertyName("colors")] public List<string> Colors { get; set; }

        [JsonPropertyName("flavor_text")] public string FlavorText { get; set; }

        [JsonPropertyName("artist")] public string Artist { get; set; }

        [JsonPropertyName("artist_id")] public string ArtistId { get; set; }

        [JsonPropertyName("illustration_id")] public string IllustrationId { get; set; }

        [JsonPropertyName("image_uris")] public ImageUris ImageUris { get; set; }

        [JsonPropertyName("power")] public string Power { get; set; }

        [JsonPropertyName("toughness")] public string Toughness { get; set; }

        [JsonPropertyName("loyalty")] public string Loyalty { get; set; }
    }
}