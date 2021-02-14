using System;

namespace Infrastructure.Models.tmp.Review {
    public class ReviewValues : IEquatable<ReviewValues> {
        public int Kal { get; set; }
        public string Name { get; set; }
        public string Cost { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        public string Nizzahon { get; set; }
        public string Draftsim { get; set; }
        public string MtgazDrift { get; set; }
        public string MtgazRas { get; set; }
        public string LrMarshal { get; set; }
        public string Lrlsv { get; set; }
        public string Da { get; set; }
        public int NizzaScore { get; set; }
        public int DraftsimScore { get; set; }
        public int DriftScore { get; set; }
        public int RasScore { get; set; }
        public int MarshScore { get; set; }
        public int LsvScore { get; set; }
        public int DaScore { get; set; }
        public double AvgScore { get; set; }
        public double StDev { get; set; }

        public bool Equals(ReviewValues other) {
            return Name == other.Name;
        }

        public override bool Equals(object obj) {
            return Equals((ReviewValues) obj);
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }
}
