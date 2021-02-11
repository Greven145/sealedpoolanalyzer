using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Logic.models.Scryfall
{
    public class SetCard : IEquatable<SetCard>
    {
        [JsonPropertyName("object")] public string Object { get; set; }

        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("oracle_id")] public string OracleId { get; set; }

        [JsonPropertyName("multiverse_ids")] public List<int> MultiverseIds { get; set; }

        [JsonPropertyName("mtgo_id")] public int MtgoId { get; set; }

        [JsonPropertyName("arena_id")] public int ArenaId { get; set; }

        [JsonPropertyName("tcgplayer_id")] public int TcgplayerId { get; set; }

        [JsonPropertyName("cardmarket_id")] public int CardmarketId { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("lang")] public string Lang { get; set; }

        [JsonPropertyName("released_at")] public string ReleasedAt { get; set; }

        [JsonPropertyName("uri")] public string Uri { get; set; }

        [JsonPropertyName("scryfall_uri")] public string ScryfallUri { get; set; }

        [JsonPropertyName("layout")] public string Layout { get; set; }

        [JsonPropertyName("highres_image")] public bool HighresImage { get; set; }

        [JsonPropertyName("image_uris")] public ImageUris ImageUris { get; set; }

        [JsonPropertyName("mana_cost")] public string ManaCost { get; set; }

        [JsonPropertyName("cmc")] public int Cmc { get; set; }

        [JsonPropertyName("type_line")] public string TypeLine { get; set; }

        [JsonPropertyName("oracle_text")] public string OracleText { get; set; }

        [JsonPropertyName("power")] public string Power { get; set; }

        [JsonPropertyName("toughness")] public string Toughness { get; set; }

        [JsonPropertyName("colors")] public List<string> Colors { get; set; }

        [JsonPropertyName("color_identity")] public List<string> ColorIdentity { get; set; }

        [JsonPropertyName("keywords")] public List<string> Keywords { get; set; }

        [JsonPropertyName("legalities")] public Legalities Legalities { get; set; }

        [JsonPropertyName("games")] public List<string> Games { get; set; }

        [JsonPropertyName("reserved")] public bool Reserved { get; set; }

        [JsonPropertyName("foil")] public bool Foil { get; set; }

        [JsonPropertyName("nonfoil")] public bool Nonfoil { get; set; }

        [JsonPropertyName("oversized")] public bool Oversized { get; set; }

        [JsonPropertyName("promo")] public bool Promo { get; set; }

        [JsonPropertyName("reprint")] public bool Reprint { get; set; }

        [JsonPropertyName("variation")] public bool Variation { get; set; }

        [JsonPropertyName("set")] public string Set { get; set; }

        [JsonPropertyName("set_name")] public string SetName { get; set; }

        [JsonPropertyName("set_type")] public string SetType { get; set; }

        [JsonPropertyName("set_uri")] public string SetUri { get; set; }

        [JsonPropertyName("set_search_uri")] public string SetSearchUri { get; set; }

        [JsonPropertyName("scryfall_set_uri")] public string ScryfallSetUri { get; set; }

        [JsonPropertyName("rulings_uri")] public string RulingsUri { get; set; }

        [JsonPropertyName("prints_search_uri")]
        public string PrintsSearchUri { get; set; }

        [JsonPropertyName("collector_number")] public string CollectorNumber { get; set; }

        [JsonPropertyName("digital")] public bool Digital { get; set; }

        [JsonPropertyName("rarity")] public string Rarity { get; set; }

        [JsonPropertyName("flavor_text")] public string FlavorText { get; set; }

        [JsonPropertyName("card_back_id")] public string CardBackId { get; set; }

        [JsonPropertyName("artist")] public string Artist { get; set; }

        [JsonPropertyName("artist_ids")] public List<string> ArtistIds { get; set; }

        [JsonPropertyName("illustration_id")] public string IllustrationId { get; set; }

        [JsonPropertyName("border_color")] public string BorderColor { get; set; }

        [JsonPropertyName("frame")] public string Frame { get; set; }

        [JsonPropertyName("frame_effects")] public List<string> FrameEffects { get; set; }

        [JsonPropertyName("full_art")] public bool FullArt { get; set; }

        [JsonPropertyName("textless")] public bool Textless { get; set; }

        [JsonPropertyName("booster")] public bool Booster { get; set; }

        [JsonPropertyName("story_spotlight")] public bool StorySpotlight { get; set; }

        [JsonPropertyName("edhrec_rank")] public int EdhrecRank { get; set; }

        [JsonPropertyName("prices")] public Prices Prices { get; set; }

        [JsonPropertyName("related_uris")] public RelatedUris RelatedUris { get; set; }

        [JsonPropertyName("preview")] public Preview Preview { get; set; }

        [JsonPropertyName("all_parts")] public List<AllPart> AllParts { get; set; }

        [JsonPropertyName("promo_types")] public List<string> PromoTypes { get; set; }

        [JsonPropertyName("produced_mana")] public List<string> ProducedMana { get; set; }

        [JsonPropertyName("card_faces")] public List<CardFace> CardFaces { get; set; }

        [JsonPropertyName("watermark")] public string Watermark { get; set; }

        [JsonPropertyName("loyalty")] public string Loyalty { get; set; }

        [JsonPropertyName("printed_name")] public string PrintedName { get; set; }

        [JsonPropertyName("printed_type_line")]
        public string PrintedTypeLine { get; set; }

        [JsonPropertyName("printed_text")] public string PrintedText { get; set; }

        public bool Equals(SetCard other) {
            return Name == other.Name;
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }
}