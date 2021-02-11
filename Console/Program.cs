using System;
using System.Collections.Generic;
using System.Linq;
using BetterConsoleTables;
using Logic;
using Logic.models;
using Logic.models.CardPool;
using Logic.models.Review;
using Logic.models.Scryfall;

namespace Console {
    internal partial class Program {
        private static void Main(string[] args) {
            WriteHeader();
            var (deck, pool, setCards, reviewCards) = LoadData();
            System.Console.WriteLine();
            var mergedDeckData = DataAnalysis.GetMergedData(deck, setCards, reviewCards);
            WriteDeckRatings(mergedDeckData);

            var mergedPoolData = DataAnalysis.GetMergedData(pool, setCards, reviewCards);
            WritePoolRatings(mergedPoolData);
            WriteColorRatings(mergedPoolData);
            WriteGoodCardsYoureNotPlaying(mergedDeckData, mergedPoolData);
            WriteTribalRatings(mergedPoolData);
        }

        private static void WriteTribalRatings(IEnumerable<MergedData> mergedPoolData) {
            Colorful.Console.WriteAscii("Tribal Breakdown", SmallFont, BlackManaColor);

            var tribalData = DataAnalysis.GetTribalAnalysis(mergedPoolData).OrderByDescending(t => t.TribalRating);

            var table = new Table(TableConfiguration.Markdown(), Alignment.Left, Alignment.Left, "Tribe", "Total Number",
                "Average Rating", "Tribal Strength", "Colors");
            foreach (var tribe in tribalData) {
                table.AddRow(tribe.Tribe, tribe.NumberOfCreatures, DataAnalysis.GetRatingDisplay(tribe.AvgRating), tribe.TribalRating, string.Join(",",tribe.Colors));
            }

            System.Console.WriteLine(table.ToString());            
        }

        private static void WriteGoodCardsYoureNotPlaying(IEnumerable<MergedData> mergedDeckData,
            IEnumerable<MergedData> mergedPoolData) {
            Colorful.Console.WriteAscii("Consider these", SmallFont, WhiteManaColor);
            
            var (inColors, outColors) = DataAnalysis.GetCardsToConsider(mergedDeckData, mergedPoolData);
            
            Colorful.Console.WriteLine("Cards in your colors to consider", GreenManaColor);
            PrintConsiderTables(inColors);

            Colorful.Console.WriteLine("Cards that share at least one color as your pool to consider", RedManaColor);
            PrintConsiderTables(outColors);

            void PrintConsiderTables(IEnumerable<ConsiderCards> considerCards) {
                System.Console.WriteLine();
                var table = new Table(TableConfiguration.Markdown(), Alignment.Left, Alignment.Left, "Name", "Cost",
                    "Rating");
                foreach (var card in considerCards) {
                    table.AddRow(card.Name, card.Cost, DataAnalysis.GetRatingDisplay(card.Rating));
                }

                System.Console.WriteLine(table.ToString());
            }
        }

        private static void WriteColorRatings(IEnumerable<MergedData> mergedPoolData) {
            Colorful.Console.WriteAscii("Pool Color rating", SmallFont, WhiteManaColor);

            var data = DataAnalysis.GetColorRatings(mergedPoolData);

            var table = new Table(TableConfiguration.Markdown());
            table.AddColumns(Alignment.Left, Alignment.Left, data.Select(s => s.Color));
            table.AddRow(data.Select(s => s.Rating));
            System.Console.WriteLine(table.ToString());
        }

        private static void WritePoolRatings(IEnumerable<MergedData> mergedPoolData) {
            Colorful.Console.WriteAscii("Pool rating", SmallFont, WhiteManaColor);

            var basicAnalysisData = DataAnalysis.GetPoolRatings(mergedPoolData);
            var longestCardNameLength = DataAnalysis.GetLengthOfLongestCardName(basicAnalysisData.Select(b => b.Name));

            var cardsToDisplayForDeck =
                basicAnalysisData
                    .OrderByDescending(i => i.Rating)
                    .ThenBy(i => i.Name)
                    .Select((value, index) => (value, index));

            foreach (var (card, count) in cardsToDisplayForDeck) {
                System.Console.WriteLine(
                    $"{count + 1,3}) {card.Name.PadRight(longestCardNameLength + 5)} Rating: {DataAnalysis.GetRatingDisplay(card.Rating)}");
            }
        }

        private static void WriteDeckRatings(IEnumerable<MergedData> mergedData) {
            Colorful.Console.WriteAscii("Deck rating", SmallFont, WhiteManaColor);

            var basicAnalysisData = DataAnalysis.GetPoolRatings(mergedData);
            var longestCardNameLength = DataAnalysis.GetLengthOfLongestCardName(basicAnalysisData.Select(b => b.Name));

            var cardsToDisplayForDeck =
                basicAnalysisData
                    .OrderByDescending(i => i.Rating)
                    .ThenBy(i => i.Name)
                    .Select((value, index) => (value, index));

            foreach (var (card, count) in cardsToDisplayForDeck) {
                System.Console.WriteLine(
                    $"{count + 1,2}) {card.Name.PadRight(longestCardNameLength + 5)} Rating: {DataAnalysis.GetRatingDisplay(card.Rating)}");
            }
        }



        private static (IEnumerable<SealedCards> deck, IEnumerable<SealedCards> pool, List<SetCard> setCards,
            List<ReviewValues> reviewCards) LoadData() {
            var deck = GetSealedDeck();
            var pool = GetSealedPool();
            var setCards = GetSetCards();
            var reviewCards = GetReviewCards();
            return (deck, pool, setCards, reviewCards);
        }

        private static void WriteHeader() {
            Colorful.Console.WriteAscii("MTG Pool Analysis", StandardFont, WhiteManaColor);
            System.Console.WriteLine();
            System.Console.WriteLine(
                "Ratings from https://docs.google.com/spreadsheets/d/1OarV3tOFriskrpFUx8LpEJm_JwDMDnZQ_zSjjdeclms/edit#gid=0");
            System.Console.WriteLine("Set data from https://scryfall.com/docs/api/bulk-data");
            System.Console.WriteLine();
        }
    }
}