using System;
using System.Collections.Generic;
using System.Linq;
using Application.Models;
using Domain.Entities;

namespace Application.Features.DataAnalysis {
    public class DataAnalysis {
        private static readonly List<string> _colorOrder = new() {
            "W",
            "U",
            "B",
            "R",
            "G"
        };

        private static Func<MagicCard, ConsiderCards> MergedToConsiderSelector => d => new ConsiderCards
            {Name = d.Name, Cost = d.ManaCost, Rating = d.Review.Score};

        public static IEnumerable<(string Name, decimal Rating)> GetPoolRatings(IEnumerable<MagicCard> MagicCard) {
            return MagicCard.Select(m => (m.Name, Rating: m.Review?.Score ?? 0));
        }

        public static IEnumerable<(string Color, string Rating)>
            GetColorRatings(IEnumerable<MagicCard> mergedPoolData) {
            var monoColorCards = mergedPoolData.Where(m => m.Colors?.Count() == 1);
            var multiColorCards = mergedPoolData.Where(m => m.Colors?.Count() > 1);
            var colorlessCards = mergedPoolData.Where(m =>
                m.Colors.Count() < 1 &&
                !m.TypeLine.Contains("Land", StringComparison.InvariantCultureIgnoreCase));

            var white = monoColorCards.Where(m =>
                m.Colors.Contains("W", StringComparer.InvariantCultureIgnoreCase));
            var blue = monoColorCards.Where(m =>
                m.Colors.Contains("U", StringComparer.InvariantCultureIgnoreCase));
            var black = monoColorCards.Where(m =>
                m.Colors.Contains("B", StringComparer.InvariantCultureIgnoreCase));
            var red = monoColorCards.Where(m =>
                m.Colors.Contains("R", StringComparer.InvariantCultureIgnoreCase));
            var green = monoColorCards.Where(m =>
                m.Colors.Contains("G", StringComparer.InvariantCultureIgnoreCase));

            var multiColorCombinations = multiColorCards.GroupBy(m => string.Join("/", m.Colors));

            var headers = new List<string> {"White", "Blue", "Black", "Red", "Green", "Colorless"};
            headers.AddRange(multiColorCombinations.Select(s => s.Key));
            var row = new List<string> {
                GetRatingDisplay(Math.Round(white.Average(c => c.Review.Score), 4)),
                GetRatingDisplay(Math.Round(blue.Average(c => c.Review.Score), 4)),
                GetRatingDisplay(Math.Round(black.Average(c => c.Review.Score), 4)),
                GetRatingDisplay(Math.Round(red.Average(c => c.Review.Score), 4)),
                GetRatingDisplay(Math.Round(green.Average(c => c.Review.Score), 4)),
                GetRatingDisplay(Math.Round(colorlessCards.Average(c => c.Review.Score), 4))
            };
            row.AddRange(
                multiColorCombinations
                    .Select(colorCombination =>
                        GetRatingDisplay(Math.Round(colorCombination.Average(c => c.Review.Score), 4))));

            return headers.Zip(row);
        }

        public static IEnumerable<TribalCards> GetTribalAnalysis(IEnumerable<MagicCard> pool) {
            var createsTypes = pool
                .Where(d => d.TypeLine.StartsWith("Creature — "))
                .Select(d => d.TypeLine.Substring(11))
                .Select(t => t.Split(' '))
                .SelectMany(t => t)
                .Distinct();
            var creaturesByType =
                createsTypes.Select(t => (Type: t, Creatures: pool.Where(d => d.TypeLine.Contains(t))));
            return creaturesByType.Select(d => new TribalCards {
                AvgRating = Math.Round(d.Creatures.Average(c => c.Review.Score), 4),
                NumberOfCreatures = d.Creatures.Count(),
                Tribe = d.Type,
                TribalRating = Math.Round(d.Creatures.Count() * d.Creatures.Average(c => c.Review.Score)),
                Colors = d.Creatures
                    .Where(d => d.Colors is not null)
                    .SelectMany(c => c.Colors)
                    .Distinct()
                    .OrderBy(c => _colorOrder.IndexOf(c))
            });
        }

        public static (IEnumerable<ConsiderCards> InColorCards, IEnumerable<ConsiderCards> SharedColorCards)
            GetCardsToConsider(IEnumerable<MagicCard> deck, IEnumerable<MagicCard> pool) {
            var deckColors = deck
                .Where(d => d.Colors is not null)
                .SelectMany(d => d.Colors)
                .Distinct().ToList();
            var possiblePoolCards = pool
                .Distinct()
                .Where(d => d.Colors is not null)
                .Where(d => !deck.Select(c => c.Name).Contains(d.Name))
                .Where(d => d.Review.Score > 6)
                .OrderByDescending(d => d.Review.Score)
                .ThenBy(d => d.Name);

            var inColors = possiblePoolCards
                .Where(d => d.Colors.Any() && d.Colors.All(c => deckColors.Contains(c)))
                .Select(MergedToConsiderSelector);
            var outColors = possiblePoolCards
                .Where(d => d.Colors.Any() && d.Colors.Any(c => deckColors.Contains(c)))
                .Where(d => !inColors.Select(c => c.Name).Contains(d.Name))
                .Select(MergedToConsiderSelector);

            return (inColors, outColors);
        }

        public static int GetLengthOfLongestCardName(IEnumerable<string> cardNames) {
            return cardNames.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
        }

        public static string GetRatingDisplay(decimal rating) {
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
