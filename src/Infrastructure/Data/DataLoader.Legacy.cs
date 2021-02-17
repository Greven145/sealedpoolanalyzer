using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Infrastructure.Models.tmp.CardPool;
using TinyCsvParser;

namespace Infrastructure.Data {
    public partial class DataLoader {
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
