using System.Collections.Generic;

namespace Application.Models {
    public class TribalCards {
        public string Tribe { get; set; }
        public int NumberOfCreatures { get; set; }
        public decimal AvgRating { get; set; }
        public decimal TribalRating { get; set; }
        public IEnumerable<string> Colors { get; set; }
    }
}
