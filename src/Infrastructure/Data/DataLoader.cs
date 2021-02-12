using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Models.tmp.CardPool;
using Application.Models.tmp.Review;
using Application.Models.tmp.Scryfall;
using CsvHelper;
using CsvHelper.Configuration;
using TinyCsvParser;

namespace Infrastructure.Data {
    public class DataLoader {
        public static List<ReviewValues> GetReviewCards(string filePath) {
            using var reader = new StreamReader(filePath);
            return GetReviewCards(reader);
        }

        public static List<ReviewValues> GetReviewCards(StreamReader streamReader) {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                NewLine = Environment.NewLine,
                Quote = '"',
                Delimiter = ","
            };
            using var csv = new CsvReader(streamReader, config);
            csv.Context.RegisterClassMap<ReviewValuesClassMap>();
            var reviewCards = csv.GetRecords<ReviewValues>().Distinct().ToList();
            return reviewCards;
        }

        public static List<SetCard> GetSetCards(string filePath) {
            var setCards = JsonSerializer.Deserialize<List<SetCard>>(File.ReadAllText(filePath));
            setCards = setCards
                .Where(s => s.FrameEffects != null && !s.FrameEffects.Contains("showcase") || s.FrameEffects is null)
                .Distinct().ToList();
            return setCards;
        }

        public static IEnumerable<SealedCards> GetSealedPool(string filePath) {
            var sealedCards = GetCardsFromSealedCsv(filePath);
            var unique = sealedCards.Distinct().Count();
            return sealedCards;
        }

        public static IEnumerable<SealedCards> GetSealedDeck(string filePath) {
            var sealedCards = GetCardsFromSealedCsv(filePath);
            var unique = sealedCards.Distinct().Count();
            return sealedCards;
        }

        public static IEnumerable<SealedCards> GetCardsFromSealedCsv(string filename) {
            using var reader = new StreamReader(filename);
            return GetCardsFromSealedCsv(reader);
        }

        public static IEnumerable<SealedCards> GetCardsFromSealedCsv(StreamReader streamReader) {
            return GetCardsFromString(streamReader.ReadToEnd());
        }

        public static async Task<IEnumerable<SealedCards>> GetCardsFromSealedCsvAsync(StreamReader streamReader) {
            var fileContent = await streamReader.ReadToEndAsync();
            fileContent = Regex.Replace(fileContent, @"\r\n?|\n", Environment.NewLine);

            return GetCardsFromString(fileContent);
        }

        public static IEnumerable<SealedCards> GetCardsFromString(string fileContent) {
            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvMapper = new CsvSealedCardsMapping();
            var csvParser = new CsvParser<SealedCards>(csvParserOptions, csvMapper);
            var csvReaderOptions = new CsvReaderOptions(new[] {Environment.NewLine});

            var csvParseResult = csvParser.ReadFromString(csvReaderOptions, fileContent).ToList();
            var sealedCards = csvParseResult.Select(r => r.Result).ToList();

            var tempList = new List<SealedCards>();

            foreach (var duplicates in sealedCards.Where(s => s.TotalQty > 1)) {
                var totalQuantity = duplicates.TotalQty;
                duplicates.TotalQty = 1;
                duplicates.RegQty = 1;
                duplicates.FoilQty = 1;

                for (var x = 1; x < totalQuantity; x++) {
                    tempList.Add(duplicates.ShallowCopy());
                }
            }

            sealedCards.AddRange(tempList);

            return sealedCards;
        }
    }
}
