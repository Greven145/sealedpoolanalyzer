using System;

namespace Domain.Entities {
    public class MagicCardReview {
        public Guid Id { get; set; }
        public MagicCard Card { get; set; }
        public decimal Score { get; set; }
        
        public Guid MagicCardId { get; set; }
    }
}
