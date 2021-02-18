﻿using Newtonsoft.Json;

namespace Infrastructure.Models.tmp.Scryfall {
    public class RelatedUris {
        [JsonProperty("gatherer")] public string Gatherer { get; set; }

        [JsonProperty("tcgplayer_decks")] public string TcgplayerDecks { get; set; }

        [JsonProperty("edhrec")] public string Edhrec { get; set; }

        [JsonProperty("mtgtop8")] public string Mtgtop8 { get; set; }
    }
}
