using System;

namespace Logic.models.CardPool
{
    public class SealedCards : IEquatable<SealedCards>
    {
        public int TotalQty { get; set; }
        public int RegQty { get; set; }
        public int FoilQty { get; set; }
        public string Card { get; set; }
        public string Set { get; set; }
        public string ManaCost { get; set; }
        public string CardType { get; set; }
        public string Color { get; set; }
        public string Rarity { get; set; }
        public int Mvid { get; set; }
        public double SinglePrice { get; set; }
        public double SingleFoilPrice { get; set; }
        public double TotalPrice { get; set; }
        public string PriceSource { get; set; }
        public string Notes { get; set; }
        
        
        public SealedCards ShallowCopy()
        {
            return (SealedCards) MemberwiseClone();
        }

        public bool Equals(SealedCards other) {
            return Card == other.Card;
        }

        public override int GetHashCode() {
            return Card.GetHashCode();
        }
    }
}
