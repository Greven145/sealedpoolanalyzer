using System.Collections.Generic;

namespace Domain.Entities {
    public class MagicCard {
        public string Name { get; set; }
        public IEnumerable<int> MultiverseIds { get; set; }
        public string ManaCost { get; set; }
        public string TypeLine { get; set; }
        public IEnumerable<string> Colors { get; set; }
        public Set Sets { get; set; }
        public MagicCardReview Review { get; set; }
    }
}
