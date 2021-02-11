using System;
using Logic.models.CardPool;
using Logic.models.Review;
using Logic.models.Scryfall;

namespace Logic.models
{
    public class MergedData  : IEquatable<MergedData>
    {
        public SealedCards SealedCard { get; set; }
        public SetCard SetCard { get; set; }
        public ReviewValues ReviewValues { get; set; }

        public bool Equals(MergedData other)
        {
            return SetCard.Name == other.SetCard.Name &&
                   SealedCard.Card == other.SealedCard.Card &&
                   ReviewValues.Name == other.ReviewValues.Name;
        }

        public override int GetHashCode()
        {
            int hashSealedCardName = SealedCard.Card.GetHashCode();
            int hashSetCardName = SetCard.Name.GetHashCode();
            int hasReviewValuesName = ReviewValues.Name.GetHashCode();

            //Calculate the hash code for the product.
            return hashSealedCardName ^ hashSetCardName ^ hasReviewValuesName;
        }
    }
}
