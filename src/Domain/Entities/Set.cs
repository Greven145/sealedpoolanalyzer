using System;

namespace Domain.Entities {
    public class Set {
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public MagicCard Cards { get; set; }
    }
}
