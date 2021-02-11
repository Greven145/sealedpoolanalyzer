#r "nuget: TinyCsvParser, 2.6.0"

using System.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

#region classes
private class CsvUserDetailsMapping : CsvMapping<SealedCards>
{
    public CsvUserDetailsMapping() : base()
    {
        MapProperty(0, x => x.TotalQty);
        MapProperty(1, x => x.RegQty);
        MapProperty(2, x => x.FoilQty);
        MapProperty(3, x => x.Card);
        MapProperty(4, x => x.Set);
        MapProperty(5, x => x.ManaCost);
        MapProperty(6, x => x.CardType);
        MapProperty(7, x => x.Rarity);
        MapProperty(8, x => x.Mvid);
        MapProperty(9, x => x.SinglePrice);
        MapProperty(10, x => x.SingleFoilPrice);
        MapProperty(11, x => x.TotalPrice);
        MapProperty(12, x => x.PriceSource);
        MapProperty(13, x => x.Notes);
    }
}
public class SealedCards
{
    public int TotalQty { get; set; }
    public int RegQty { get; set; }
    public int FoilQty { get; set; }
    public string Card { get; set; }
    public string Set { get; set; }
    public string ManaCost { get; set; }
    public string CardType { get; set; }
    public string Color { get; set; }
    public string Rarity { get; set; }
    public int Mvid { get; set; }
    public double SinglePrice { get; set; }
    public double SingleFoilPrice { get; set; }
    public double TotalPrice { get; set; }
    public string PriceSource { get; set; }
    public string Notes { get; set; }
}
#region Cards from API
public class ImageUris
{
    [JsonPropertyName("small")]
    public string Small { get; set; }

    [JsonPropertyName("normal")]
    public string Normal { get; set; }

    [JsonPropertyName("large")]
    public string Large { get; set; }

    [JsonPropertyName("png")]
    public string Png { get; set; }

    [JsonPropertyName("art_crop")]
    public string ArtCrop { get; set; }

    [JsonPropertyName("border_crop")]
    public string BorderCrop { get; set; }
}
public class Legalities
{
    [JsonPropertyName("standard")]
    public string Standard { get; set; }

    [JsonPropertyName("future")]
    public string Future { get; set; }

    [JsonPropertyName("historic")]
    public string Historic { get; set; }

    [JsonPropertyName("gladiator")]
    public string Gladiator { get; set; }

    [JsonPropertyName("pioneer")]
    public string Pioneer { get; set; }

    [JsonPropertyName("modern")]
    public string Modern { get; set; }

    [JsonPropertyName("legacy")]
    public string Legacy { get; set; }

    [JsonPropertyName("pauper")]
    public string Pauper { get; set; }

    [JsonPropertyName("vintage")]
    public string Vintage { get; set; }

    [JsonPropertyName("penny")]
    public string Penny { get; set; }

    [JsonPropertyName("commander")]
    public string Commander { get; set; }

    [JsonPropertyName("brawl")]
    public string Brawl { get; set; }

    [JsonPropertyName("duel")]
    public string Duel { get; set; }

    [JsonPropertyName("oldschool")]
    public string Oldschool { get; set; }

    [JsonPropertyName("premodern")]
    public string Premodern { get; set; }
}
public class Prices
{
    [JsonPropertyName("usd")]
    public string Usd { get; set; }

    [JsonPropertyName("usd_foil")]
    public string UsdFoil { get; set; }

    [JsonPropertyName("eur")]
    public string Eur { get; set; }

    [JsonPropertyName("eur_foil")]
    public string EurFoil { get; set; }

    [JsonPropertyName("tix")]
    public string Tix { get; set; }
}
public class RelatedUris
{
    [JsonPropertyName("gatherer")]
    public string Gatherer { get; set; }

    [JsonPropertyName("tcgplayer_decks")]
    public string TcgplayerDecks { get; set; }

    [JsonPropertyName("edhrec")]
    public string Edhrec { get; set; }

    [JsonPropertyName("mtgtop8")]
    public string Mtgtop8 { get; set; }
}
public class Preview
{
    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("source_uri")]
    public string SourceUri { get; set; }

    [JsonPropertyName("previewed_at")]
    public string PreviewedAt { get; set; }
}
public class AllPart
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("component")]
    public string Component { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type_line")]
    public string TypeLine { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}
public class CardFace
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("mana_cost")]
    public string ManaCost { get; set; }

    [JsonPropertyName("type_line")]
    public string TypeLine { get; set; }

    [JsonPropertyName("oracle_text")]
    public string OracleText { get; set; }

    [JsonPropertyName("colors")]
    public List<string> Colors { get; set; }

    [JsonPropertyName("flavor_text")]
    public string FlavorText { get; set; }

    [JsonPropertyName("artist")]
    public string Artist { get; set; }

    [JsonPropertyName("artist_id")]
    public string ArtistId { get; set; }

    [JsonPropertyName("illustration_id")]
    public string IllustrationId { get; set; }

    [JsonPropertyName("image_uris")]
    public ImageUris ImageUris { get; set; }

    [JsonPropertyName("power")]
    public string Power { get; set; }

    [JsonPropertyName("toughness")]
    public string Toughness { get; set; }

    [JsonPropertyName("loyalty")]
    public string Loyalty { get; set; }
}
public class ApiCard
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("oracle_id")]
    public string OracleId { get; set; }

    [JsonPropertyName("multiverse_ids")]
    public List<int> MultiverseIds { get; set; }

    [JsonPropertyName("mtgo_id")]
    public int MtgoId { get; set; }

    [JsonPropertyName("arena_id")]
    public int ArenaId { get; set; }

    [JsonPropertyName("tcgplayer_id")]
    public int TcgplayerId { get; set; }

    [JsonPropertyName("cardmarket_id")]
    public int CardmarketId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("lang")]
    public string Lang { get; set; }

    [JsonPropertyName("released_at")]
    public string ReleasedAt { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    [JsonPropertyName("scryfall_uri")]
    public string ScryfallUri { get; set; }

    [JsonPropertyName("layout")]
    public string Layout { get; set; }

    [JsonPropertyName("highres_image")]
    public bool HighresImage { get; set; }

    [JsonPropertyName("image_uris")]
    public ImageUris ImageUris { get; set; }

    [JsonPropertyName("mana_cost")]
    public string ManaCost { get; set; }

    [JsonPropertyName("cmc")]
    public int Cmc { get; set; }

    [JsonPropertyName("type_line")]
    public string TypeLine { get; set; }

    [JsonPropertyName("oracle_text")]
    public string OracleText { get; set; }

    [JsonPropertyName("power")]
    public string Power { get; set; }

    [JsonPropertyName("toughness")]
    public string Toughness { get; set; }

    [JsonPropertyName("colors")]
    public List<string> Colors { get; set; }

    [JsonPropertyName("color_identity")]
    public List<string> ColorIdentity { get; set; }

    [JsonPropertyName("keywords")]
    public List<string> Keywords { get; set; }

    [JsonPropertyName("legalities")]
    public Legalities Legalities { get; set; }

    [JsonPropertyName("games")]
    public List<string> Games { get; set; }

    [JsonPropertyName("reserved")]
    public bool Reserved { get; set; }

    [JsonPropertyName("foil")]
    public bool Foil { get; set; }

    [JsonPropertyName("nonfoil")]
    public bool Nonfoil { get; set; }

    [JsonPropertyName("oversized")]
    public bool Oversized { get; set; }

    [JsonPropertyName("promo")]
    public bool Promo { get; set; }

    [JsonPropertyName("reprint")]
    public bool Reprint { get; set; }

    [JsonPropertyName("variation")]
    public bool Variation { get; set; }

    [JsonPropertyName("set")]
    public string Set { get; set; }

    [JsonPropertyName("set_name")]
    public string SetName { get; set; }

    [JsonPropertyName("set_type")]
    public string SetType { get; set; }

    [JsonPropertyName("set_uri")]
    public string SetUri { get; set; }

    [JsonPropertyName("set_search_uri")]
    public string SetSearchUri { get; set; }

    [JsonPropertyName("scryfall_set_uri")]
    public string ScryfallSetUri { get; set; }

    [JsonPropertyName("rulings_uri")]
    public string RulingsUri { get; set; }

    [JsonPropertyName("prints_search_uri")]
    public string PrintsSearchUri { get; set; }

    [JsonPropertyName("collector_number")]
    public string CollectorNumber { get; set; }

    [JsonPropertyName("digital")]
    public bool Digital { get; set; }

    [JsonPropertyName("rarity")]
    public string Rarity { get; set; }

    [JsonPropertyName("flavor_text")]
    public string FlavorText { get; set; }

    [JsonPropertyName("card_back_id")]
    public string CardBackId { get; set; }

    [JsonPropertyName("artist")]
    public string Artist { get; set; }

    [JsonPropertyName("artist_ids")]
    public List<string> ArtistIds { get; set; }

    [JsonPropertyName("illustration_id")]
    public string IllustrationId { get; set; }

    [JsonPropertyName("border_color")]
    public string BorderColor { get; set; }

    [JsonPropertyName("frame")]
    public string Frame { get; set; }

    [JsonPropertyName("frame_effects")]
    public List<string> FrameEffects { get; set; }

    [JsonPropertyName("full_art")]
    public bool FullArt { get; set; }

    [JsonPropertyName("textless")]
    public bool Textless { get; set; }

    [JsonPropertyName("booster")]
    public bool Booster { get; set; }

    [JsonPropertyName("story_spotlight")]
    public bool StorySpotlight { get; set; }

    [JsonPropertyName("edhrec_rank")]
    public int EdhrecRank { get; set; }

    [JsonPropertyName("prices")]
    public Prices Prices { get; set; }

    [JsonPropertyName("related_uris")]
    public RelatedUris RelatedUris { get; set; }

    [JsonPropertyName("preview")]
    public Preview Preview { get; set; }

    [JsonPropertyName("all_parts")]
    public List<AllPart> AllParts { get; set; }

    [JsonPropertyName("promo_types")]
    public List<string> PromoTypes { get; set; }

    [JsonPropertyName("produced_mana")]
    public List<string> ProducedMana { get; set; }

    [JsonPropertyName("card_faces")]
    public List<CardFace> CardFaces { get; set; }

    [JsonPropertyName("watermark")]
    public string Watermark { get; set; }

    [JsonPropertyName("loyalty")]
    public string Loyalty { get; set; }

    [JsonPropertyName("printed_name")]
    public string PrintedName { get; set; }

    [JsonPropertyName("printed_type_line")]
    public string PrintedTypeLine { get; set; }

    [JsonPropertyName("printed_text")]
    public string PrintedText { get; set; }
}
#endregion
#region Cards from Review
public class Flags
{
    [JsonPropertyName("sideboard")]
    public bool Sideboard { get; set; }

    [JsonPropertyName("synergy")]
    public bool Synergy { get; set; }

    [JsonPropertyName("buildaround")]
    public bool Buildaround { get; set; }
}
public class ReviewCard
{
    [JsonPropertyName("card_id")]
    public int CardId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("rarity")]
    public string Rarity { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("tier")]
    public string Tier { get; set; }

    [JsonPropertyName("sort_key")]
    public int SortKey { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    [JsonPropertyName("types")]
    public List<string> Types { get; set; }

    [JsonPropertyName("cmc")]
    public double Cmc { get; set; }

    [JsonPropertyName("flags")]
    public Flags Flags { get; set; }
}
#endregion
#endregion

static readonly List<String> ratingSortOrder = new List<String> {
    "A+",
    "A",
    "A-",
    "B+",
    "B",
    "B-",
    "C+",
    "C",
    "C-",
    "D+",
    "D",
    "D-",
    "F",
    "TBD"
};


//csv parser
//fix file names
//create data subfolder
//var sealedCards = JsonSerializer.Deserialize<List<SealedCards>>(File.ReadAllText("KaldheimSealedLeague.json"));
CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
CsvUserDetailsMapping csvMapper = new CsvUserDetailsMapping();
CsvParser<userdetails> csvParser = new CsvParser<SealedCards>(csvParserOptions, csvMapper);
var sealedCards = csvParser.ReadFromFile(@"KaldheimSealedLeague.csv", Encoding.ASCII).ToList();
var apiCards = JsonSerializer.Deserialize<List<ApiCard>>(File.ReadAllText("kaldheimcards.json"));
var reviewCards = JsonSerializer.Deserialize<List<ReviewCard>>(File.ReadAllText("commonuncommon.json"));
var mergedData = sealedCards.Select(s => new
{
    Name = s.Card,
    // ArenaId = apiCards.Where(a => a.Name == s.Card).Select(a => a.ArenaId).First(),
    Rating = reviewCards.Where(r => r.CardId == apiCards.Where(a => a.Name == s.Card).Select(a => a.ArenaId).First()).Select(r => r.Tier).First()
});

var longestCardNameLength = mergedData.Select(m => m.Name).Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;

foreach (var card in mergedData.OrderBy(i => ratingSortOrder.IndexOf(i.Rating)))
{
    // Console.WriteLine($"ArenaId: {card.ArenaId} Name: {card.Name} Rating: {card.Rating}");
    Console.WriteLine($"Name: {card.Name.PadRight(longestCardNameLength + 5)} Rating: {card.Rating}");
}
Console.WriteLine(apiCards.Count)