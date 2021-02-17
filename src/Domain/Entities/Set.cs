using System;
using System.Collections.Generic;

namespace Domain.Entities {
    public class Set {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public string Code { get; set; }
        public DateTimeOffset DateLoaded { get; set; }
        
        public ICollection<MagicCard> MagicCards { get; set; }
    }
}
