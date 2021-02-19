using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Parsers.Pipeline;
using Infrastructure.Models;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Infrastructure.Data.Parsers {
    public class DeckedBuilderCsvParser  : DeckParser<FileParserContext> {
        /// <summary>
        /// Dunno why, but kaldheim cards from Decked Builder are the below above
        /// the actual value
        /// </summary>
        private const int _multiverseIdOffset = 740979;
        private static readonly CsvSealedCardsMapping _mapper = new ();
        private static readonly CsvParserOptions _parserOptions = new(true, ',');
        private static readonly CsvReaderOptions _readerOptions = new(new[] {Environment.NewLine, "\n"});

        protected override async ValueTask<IEnumerable<CardFromFile>> Execute(FileParserContext context, Func<FileParserContext, ValueTask<IEnumerable<CardFromFile>>> next) {
            var fileExtension = Path.GetExtension(context.Name);

            if (string.IsNullOrEmpty(fileExtension) ||
                !fileExtension.Equals(".csv", StringComparison.InvariantCultureIgnoreCase)) {
                return await next(context);
            }

            var fileContent = Encoding.ASCII.GetString(context.Content);
                
            var csvParser = new CsvParser<SealedCards>(_parserOptions, _mapper);
            var csvParseResult = csvParser.ReadFromString(_readerOptions, fileContent).ToList();
            var sealedCards = csvParseResult.Select(r => r.Result).ToList();

            var tempList = new List<SealedCards>();

            foreach (var duplicates in sealedCards.Where(s => s.TotalQty > 1)) {
                var totalQuantity = duplicates.TotalQty;
                duplicates.TotalQty = 1;

                for (var x = 1; x < totalQuantity; x++) {
                    tempList.Add(duplicates.ShallowCopy());
                }
            }

            sealedCards.AddRange(tempList);

            context.Cards = sealedCards.Select(s => new CardFromFile {
                Name = s.Name,
                Quantity = 1,
                MultiverseId = s.Mvid - _multiverseIdOffset
            });
            return context.Cards;

        }
        
        private class SealedCards {
            public int TotalQty { get; set; }
            public string Name { get; set; }
            public int Mvid { get; set; }

            public SealedCards ShallowCopy() {
                return (SealedCards) MemberwiseClone();
            }
        }
        
        private class CsvSealedCardsMapping : CsvMapping<SealedCards> {
            public CsvSealedCardsMapping() {
                MapProperty(0, x => x.TotalQty);
                MapProperty(3, x => x.Name);
                MapProperty(9, x => x.Mvid);
            }
        }
    }
}
