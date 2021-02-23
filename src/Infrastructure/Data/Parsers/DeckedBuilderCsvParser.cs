using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Infrastructure.Data.Parsers.Pipeline.Bases;
using Infrastructure.Models;
using Persistence.Contracts;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Infrastructure.Data.Parsers {
    public class DeckedBuilderCsvParser : DeckedBuilderParser {
        private static readonly CsvSealedCardsMapping _mapper = new();
        private static readonly CsvParserOptions _parserOptions = new(true, ',');
        private static readonly CsvReaderOptions _readerOptions = new(new[] {Environment.NewLine, "\n"});

        public DeckedBuilderCsvParser(ICardRepository cardRepository, IDb2ScryfallRepository db2ScryfallRepository) :
            base(cardRepository, db2ScryfallRepository) {
        }

        protected override async ValueTask<IEnumerable<CardFromFile>> Execute(FileParserContext context,
            Func<FileParserContext, ValueTask<IEnumerable<CardFromFile>>> next) {
            var fileExtension = Path.GetExtension(context.Name);

            if (string.IsNullOrEmpty(fileExtension) ||
                !fileExtension.Equals(".csv", StringComparison.InvariantCultureIgnoreCase)) {
                return await next(context);
            }

            var fileContent = Encoding.ASCII.GetString(context.Content);

            var csvParser = new CsvParser<Item>(_parserOptions, _mapper);
            var csvParseResult = csvParser.ReadFromString(_readerOptions, fileContent).ToList();
            var sealedCards = csvParseResult.Select(r => r.Result).ToList();

            context.Cards = await GetCardFromFilesFromItemsCollection(sealedCards);
            return context.Cards;
        }

        private class CsvSealedCardsMapping : CsvMapping<Item> {
            public CsvSealedCardsMapping() {
                MapProperty(0, x => x.r);
                MapProperty(9, x => x.id);
            }
        }
    }
}
