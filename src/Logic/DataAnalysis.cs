using System;
using System.Collections.Generic;
using System.Linq;
using Logic.models;
using Logic.models.CardPool;
using Logic.models.Review;
using Logic.models.Scryfall;

namespace Logic {
    public class DataAnalysis {
        private static Func<MergedData, ConsiderCards> MergedToConsiderSelector => d => new ConsiderCards{Name = d.SealedCard.Card, Cost = d.SetCard.ManaCost, Rating = d.ReviewValues.AvgScore};
        private static readonly List<string> ColorOrder = new() {
            "W",
            "U",
            "B",
            "R",
            "G"
        }; 
        
        public static IEnumerable<MergedData> GetMergedData(IEnumerable<SealedCards> deck, List<SetCard> setCards,
            List<ReviewValues> reviewCards) {
            return from sealedCard in deck
                join setCard in setCards on sealedCard.Card equals setCard.Name into sgj
                join reviewCard in reviewCards on sealedCard.Card equals reviewCard.Name into rgj
                from subreview in rgj.DefaultIfEmpty()
                from subset in sgj.DefaultIfEmpty()
                select new MergedData {SetCard = subset, ReviewValues = subreview, SealedCard = sealedCard};
        }

        public static IEnumerable<(string Name, double Rating)> GetPoolRatings(IEnumerable<MergedData> mergedData) {
            return mergedData.Select(m => (Name: m.SealedCard.Card, Rating: m.ReviewValues?.AvgScore ?? 0));
        }

        public static IEnumerable<(string Color, string Rating)>
            GetColorRatings(IEnumerable<MergedData> mergedPoolData) {
            var monoColorCards = mergedPoolData.Where(m => m.SetCard.Colors?.Count == 1);
            var multiColorCards = mergedPoolData.Where(m => m.SetCard.Colors?.Count > 1);
            var colorlessCards = mergedPoolData.Where(m =>
                m.SetCard.Colors?.Count < 1 &&
                !m.SetCard.TypeLine.Contains("Land", StringComparison.InvariantCultureIgnoreCase));

            var white = monoColorCards.Where(m =>
                m.SetCard.Colors.Contains("W", StringComparer.InvariantCultureIgnoreCase));
            var blue = monoColorCards.Where(m =>
                m.SetCard.Colors.Contains("U", StringComparer.InvariantCultureIgnoreCase));
            var black = monoColorCards.Where(m =>
                m.SetCard.Colors.Contains("B", StringComparer.InvariantCultureIgnoreCase));
            var red = monoColorCards.Where(m =>
                m.SetCard.Colors.Contains("R", StringComparer.InvariantCultureIgnoreCase));
            var green = monoColorCards.Where(m =>
                m.SetCard.Colors.Contains("G", StringComparer.InvariantCultureIgnoreCase));

            var multiColorCombinations = multiColorCards.GroupBy(m => string.Join("/", m.SetCard.Colors));

            var headers = new List<string> {"White", "Blue", "Black", "Red", "Green", "Colorless"};
            headers.AddRange(multiColorCombinations.Select(s => s.Key));
            var row = new List<string> {
                GetRatingDisplay(Math.Round(white.Average(c => c.ReviewValues.AvgScore), 4)),
                GetRatingDisplay(Math.Round(blue.Average(c => c.ReviewValues.AvgScore), 4)),
                GetRatingDisplay(Math.Round(black.Average(c => c.ReviewValues.AvgScore), 4)),
                GetRatingDisplay(Math.Round(red.Average(c => c.ReviewValues.AvgScore), 4)),
                GetRatingDisplay(Math.Round(green.Average(c => c.ReviewValues.AvgScore), 4)),
                GetRatingDisplay(Math.Round(colorlessCards.Average(c => c.ReviewValues.AvgScore), 4))
            };
            row.AddRange(
                multiColorCombinations
                    .Select(colorCombination =>
                        GetRatingDisplay(Math.Round(colorCombination.Average(c => c.ReviewValues.AvgScore), 4))));

            return headers.Zip(row);
        }

        public static IEnumerable<TribalCards> GetTribalAnalysis(IEnumerable<MergedData> pool) {
            var createsTypes = pool
                .Where(d => d.SetCard.TypeLine.StartsWith("Creature — "))
                .Select(d => d.SetCard.TypeLine.Substring(11))
                .Select(t => t.Split(' '))
                .SelectMany(t => t)
                .Distinct();
            var creaturesByType =
                createsTypes.Select(t => (Type:t, Creatures: pool.Where(d => d.SetCard.TypeLine.Contains(t))));
            return creaturesByType.Select(d => new TribalCards {
                AvgRating = Math.Round(d.Creatures.Average(c => c.ReviewValues.AvgScore), 4),
                NumberOfCreatures = d.Creatures.Count(),
                Tribe = d.Type,
                TribalRating = Math.Round(d.Creatures.Count() * d.Creatures.Average(c => c.ReviewValues.AvgScore)),
                Colors = d.Creatures
                    .Where(d => d.SetCard.Colors is not null)
                    .SelectMany(c => c.SetCard.Colors)
                    .Distinct()
                    .OrderBy(c => ColorOrder.IndexOf(c))
            });
        }

        public static (IEnumerable<ConsiderCards> InColorCards, IEnumerable<ConsiderCards> SharedColorCards) GetCardsToConsider(IEnumerable<MergedData> deck, IEnumerable<MergedData> pool){
            var deckColors = deck
                .Where(d => d.SetCard.Colors is not null)
                .SelectMany(d => d.SetCard.Colors)
                .Distinct().ToList();
            var possiblePoolCards = pool
                .Distinct()
                .Where(d => d.SetCard.Colors is not null)
                .Where(d => !deck.Select(c => c.SealedCard.Card).Contains(d.SealedCard.Card))
                .Where(d => d.ReviewValues.AvgScore > 6)
                .OrderByDescending(d => d.ReviewValues.AvgScore)
                .ThenBy(d => d.SealedCard.Card);

            var inColors = possiblePoolCards
                .Where(d => d.SetCard.Colors.Count > 0 && d.SetCard.Colors.All(c => deckColors.Contains(c)))
                .Select(MergedToConsiderSelector);
            var outColors = possiblePoolCards
                .Where(d => d.SetCard.Colors.Count > 0 && d.SetCard.Colors.Any(c => deckColors.Contains(c)))
                .Where(d => !inColors.Select(c => c.Name).Contains(d.SealedCard.Card))
                .Select(MergedToConsiderSelector);

            return (inColors, outColors);
        }

        public static int GetLengthOfLongestCardName(IEnumerable<string> cardNames) {
            return cardNames.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
        }

        public static string GetRatingDisplay(double rating) {
            return rating switch {
                < 1 => $"{Math.Round(rating, 4),-7} (F )",
                < 2 => $"{Math.Round(rating, 4),-7} (D-)",
                < 3 => $"{Math.Round(rating, 4),-7} (D )",
                < 4 => $"{Math.Round(rating, 4),-7} (D+)",
                < 5 => $"{Math.Round(rating, 4),-7} (C-)",
                < 6 => $"{Math.Round(rating, 4),-7} (C )",
                < 7 => $"{Math.Round(rating, 4),-7} (C+)",
                < 8 => $"{Math.Round(rating, 4),-7} (B-)",
                < 9 => $"{Math.Round(rating, 4),-7} (B )",
                < 10 => $"{Math.Round(rating, 4),-7} (B+)",
                < 11 => $"{Math.Round(rating, 4),-7} (A-)",
                < 12 => $"{Math.Round(rating, 4),-7} (A )",
                _ => $"{Math.Round(rating, 4),-7} (A+)"
            };
        }
    }
}