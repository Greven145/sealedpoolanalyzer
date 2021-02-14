using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Infrastructure.Models.tmp.CardPool;
using Infrastructure.Models.tmp.Review;
using Infrastructure.Models.tmp.Scryfall;
using TinyCsvParser;

namespace Infrastructure.Data {
    public partial class DataLoader {
        private readonly IEnumerable<ReviewValues> _reviewCards;
        private readonly IEnumerable<Card> _setCards;

        public List<ReviewValues> GetReviewCards(string filePath) {
            using var reader = new StreamReader(filePath);
            return GetReviewCards(reader);
        }

        public List<ReviewValues> GetReviewCards(StreamReader streamReader) {
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

        public List<Card> GetSetCards(string filePath) {
            var setCards = JsonSerializer.Deserialize<List<Card>>(File.ReadAllText(filePath));
            setCards = setCards
                .Where(s => s.FrameEffects != null && !s.FrameEffects.Contains("showcase") || s.FrameEffects is null)
                .Distinct().ToList();
            return setCards;
        }

        public IEnumerable<SealedCards> GetSealedPool(string filePath) {
            var sealedCards = GetCardsFromSealedCsv(filePath);
            var unique = sealedCards.Distinct().Count();
            return sealedCards;
        }

        public IEnumerable<SealedCards> GetSealedDeck(string filePath) {
            var sealedCards = GetCardsFromSealedCsv(filePath);
            var unique = sealedCards.Distinct().Count();
            return sealedCards;
        }

        public IEnumerable<SealedCards> GetCardsFromSealedCsv(string filename) {
            using var reader = new StreamReader(filename);
            return GetCardsFromSealedCsv(reader);
        }

        public IEnumerable<SealedCards> GetCardsFromSealedCsv(StreamReader streamReader) {
            return GetCardsFromString(streamReader.ReadToEnd());
        }

        public async Task<IEnumerable<SealedCards>> GetCardsFromSealedCsvAsync(StreamReader streamReader) {
            var fileContent = await streamReader.ReadToEndAsync();
            fileContent = Regex.Replace(fileContent, @"\r\n?|\n", Environment.NewLine);

            return GetCardsFromString(fileContent);
        }

        public IEnumerable<SealedCards> GetCardsFromString(string fileContent) {
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
