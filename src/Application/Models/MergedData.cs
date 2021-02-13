using System;
using Application.Models.tmp.CardPool;
using Application.Models.tmp.Review;
using Application.Models.tmp.Scryfall;

namespace Application.Models {
    public class MergedData : IEquatable<MergedData> {
        public SealedCards SealedCard { get; set; }
        public SetCard SetCard { get; set; }
        public ReviewValues ReviewValues { get; set; }

        public bool Equals(MergedData other) {
            return SetCard.Name == other.SetCard.Name &&
                   SealedCard.Card == other.SealedCard.Card &&
                   ReviewValues.Name == other.ReviewValues.Name;
        }

        public override int GetHashCode() {
            var hashSealedCardName = SealedCard.Card.GetHashCode();
            var hashSetCardName = SetCard.Name.GetHashCode();
            var hasReviewValuesName = ReviewValues.Name.GetHashCode();

            //Calculate the hash code for the product.
            return hashSealedCardName ^ hashSetCardName ^ hasReviewValuesName;
        }
    }
}
