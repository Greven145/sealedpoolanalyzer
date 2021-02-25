﻿using Newtonsoft.Json;

namespace Infrastructure.Models.Scryfall {
    public class AllPart {
        [JsonProperty("object")] public string Object { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("component")] public string Component { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("type_line")] public string TypeLine { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }
    }
}
