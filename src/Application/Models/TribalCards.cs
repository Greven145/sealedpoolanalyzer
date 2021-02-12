using System.Collections.Generic;

namespace Application.Models {
    public class TribalCards {
        public string Tribe { get; set; }
        public int NumberOfCreatures { get; set; }
        public double AvgRating { get; set; }
        public double TribalRating { get; set; }
        public IEnumerable<string> Colors { get; set; }
    }
}
