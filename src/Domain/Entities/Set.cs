using System;
using System.Collections.Generic;

namespace Domain.Entities {
    public class Set {
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public IEnumerable<MagicCard> Cards { get; set; }
        public string Code { get; set; }
    }
}
