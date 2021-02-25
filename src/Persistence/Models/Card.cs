using System;
using System.Collections.Generic;

#nullable disable

namespace Persistence.Models
{
    public partial class Card
    {
        public long Mvid { get; set; }
        public long Gathererid { get; set; }
        public string Scryfallid { get; set; }
        public string Cardname { get; set; }
        public string Cardtype { get; set; }
        public string Manacost { get; set; }
        public string Cardtext { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Cardset { get; set; }
        public string Rarity { get; set; }
        public string Specialtype { get; set; }
        public long Transformid { get; set; }
        public string Artist { get; set; }
        public string Cardnumber { get; set; }
        public string SdartPath { get; set; }
        public string SdartMd5 { get; set; }
        public string HdartPath { get; set; }
        public string HdartMd5 { get; set; }
        public string ThumbPath { get; set; }
        public string ThumbMd5 { get; set; }
        public string Typeline { get; set; }
        public string Manasymbols { get; set; }
        public long Convertedmanacost { get; set; }
        public long Powerint { get; set; }
        public long Toughnessint { get; set; }
        public long Colororder { get; set; }
        public long Setorder { get; set; }
        public long Iswhite { get; set; }
        public long Isgreen { get; set; }
        public long Isred { get; set; }
        public long Isblack { get; set; }
        public long Isblue { get; set; }
        public long Iscolorless { get; set; }
        public long Prodwhite { get; set; }
        public long Prodgreen { get; set; }
        public long Prodred { get; set; }
        public long Prodblack { get; set; }
        public long Prodblue { get; set; }
        public long Prodcolorless { get; set; }
        public long Prodany { get; set; }
        public long Island { get; set; }
        public long Isbasicland { get; set; }
        public long Issorcery { get; set; }
        public long Isinstant { get; set; }
        public long Isplaneswalker { get; set; }
        public long Iscreature { get; set; }
        public long Isartifact { get; set; }
        public long Isenchantment { get; set; }
        public long Islegendary { get; set; }
        public long Isstandard { get; set; }
        public long Ispioneer { get; set; }
        public long Ismodern { get; set; }
        public long Isextended { get; set; }
        public long Islegacy { get; set; }
        public long Isedh { get; set; }
        public long Ispauper { get; set; }
        public long Ishistoric { get; set; }
        public long? Ordercmc { get; set; }
    }
}
