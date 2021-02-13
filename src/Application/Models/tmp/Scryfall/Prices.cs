using System.Text.Json.Serialization;

namespace Application.Models.tmp.Scryfall {
    public class Prices {
        [JsonPropertyName("usd")] public string Usd { get; set; }

        [JsonPropertyName("usd_foil")] public string UsdFoil { get; set; }

        [JsonPropertyName("eur")] public string Eur { get; set; }

        [JsonPropertyName("eur_foil")] public string EurFoil { get; set; }

        [JsonPropertyName("tix")] public string Tix { get; set; }
    }
}
