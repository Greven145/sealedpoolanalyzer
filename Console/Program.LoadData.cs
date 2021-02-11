using System.Collections.Generic;
using System.Linq;
using Logic;
using Logic.models.CardPool;
using Logic.models.Review;
using Logic.models.Scryfall;

namespace Console {
    internal partial class Program {
        public static List<ReviewValues> GetReviewCards() {
            Colorful.Console.Write("Loading review data... ", WhiteManaColor);
            var reviewCards = DataLoader.GetReviewCards(@"C:\Users\greve\Documents\kaldheim\data\fullSetReview.csv");
            Colorful.Console.WriteLine($"Found {reviewCards.Count()} unique cards in the review", WhiteManaColor);
            return reviewCards;
        }

        public static List<SetCard> GetSetCards() {
            Colorful.Console.Write("Loading set data... ", RedManaColor);
            var setCards = DataLoader.GetSetCards(@"C:\Users\greve\Documents\kaldheim\data\kaldheimcards.json");

            Colorful.Console.WriteLine($"Found {setCards.Count()} unique cards in the set (excluding showcase)",
                RedManaColor);
            return setCards;
        }

        public static IEnumerable<SealedCards> GetSealedPool() {
            Colorful.Console.Write("Loading sealed pool... ", BlueManaColor);
            var sealedCards = DataLoader.GetSealedPool(@"C:\Users\greve\Documents\kaldheim\data\pool.csv");
            var unique = sealedCards.Distinct().Count();
            Colorful.Console.WriteLine($"Found {unique} unique cards in your sealed pool ({sealedCards.Count()} total)",
                BlueManaColor);
            return sealedCards;
        }

        public static IEnumerable<SealedCards> GetSealedDeck() {
            Colorful.Console.Write("Loading deck... ", GreenManaColor);
            var sealedCards = DataLoader.GetSealedDeck(@"C:\Users\greve\Documents\kaldheim\data\deck.csv");
            var unique = sealedCards.Distinct().Count();
            Colorful.Console.WriteLine($"Found {unique} unique cards in your deck ({sealedCards.Count()} total)",
                GreenManaColor);
            return sealedCards;
        }
    }
}