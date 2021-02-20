using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using TinyCsvParser;

namespace Infrastructure.Data.Parsers {
    //public class DeckedBuilderDecParser : DeckedBuilderParser {
    //    protected override async ValueTask<IEnumerable<CardFromFile>> Execute(FileParserContext context,
    //        Func<FileParserContext, ValueTask<IEnumerable<CardFromFile>>> next) {
    //        var fileExtension = Path.GetExtension(context.Name);

    //        if (string.IsNullOrEmpty(fileExtension) ||
    //            !fileExtension.Equals(".dec", StringComparison.InvariantCultureIgnoreCase)) {
    //            return await next(context);
    //        }

    //        //var fileContent = Encoding.ASCII.GetString(context.Content);

    //        //var csvParser = new CsvParser<SealedCards>(_parserOptions, _mapper);
    //        //var csvParseResult = csvParser.ReadFromString(_readerOptions, fileContent).ToList();
    //        //var sealedCards = csvParseResult.Select(r => r.Result).ToList();

    //        //var tempList = new List<SealedCards>();

    //        //foreach (var duplicates in sealedCards.Where(s => s.TotalQty > 1)) {
    //        //    var totalQuantity = duplicates.TotalQty;
    //        //    duplicates.TotalQty = 1;

    //        //    for (var x = 1; x < totalQuantity; x++) {
    //        //        tempList.Add(duplicates.ShallowCopy());
    //        //    }
    //        //}

    //        //sealedCards.AddRange(tempList);

    //        //context.Cards = sealedCards.Select(s => new CardFromFile {
    //        //    Name = s.Name,
    //        //    Quantity = 1,
    //        //    MultiverseId = s.Mvid - _multiverseIdOffset
    //        //});
    //        //return context.Cards;
    //    }
    //}
}
