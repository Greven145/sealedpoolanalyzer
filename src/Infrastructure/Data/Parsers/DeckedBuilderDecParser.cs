using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Infrastructure.Data.Parsers.Pipeline;
using Infrastructure.Data.Parsers.Pipeline.Bases;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Persistence.Contracts;
using TinyCsvParser;

namespace Infrastructure.Data.Parsers {
    public class DeckedBuilderDecParser : DeckedBuilderParser {
        private const string parsingPattern = "^///mvid:(\\d+) qty:(\\d{1,2})";

        public DeckedBuilderDecParser(ICardRepository cardRepository, IDb2ScryfallRepository db2ScryfallRepository) :
            base(cardRepository, db2ScryfallRepository) { }

        protected override async ValueTask<IEnumerable<CardFromFile>> Execute(FileParserContext context,
            Func<FileParserContext, ValueTask<IEnumerable<CardFromFile>>> next)
        {
            var fileExtension = Path.GetExtension(context.Name);

            if (string.IsNullOrEmpty(fileExtension) ||
                !fileExtension.Equals(".dec", StringComparison.InvariantCultureIgnoreCase))
            {
                return await next(context);
            }

            var fileContent = Encoding.ASCII.GetString(context.Content);
            var deck = new List<Item>();
            var matches = Regex.Matches(fileContent, parsingPattern,RegexOptions.Multiline);
            foreach (Match match in matches) {
                deck.Add(new Item {
                    id = int.Parse(match.Groups[1].Value),
                    r = int.Parse(match.Groups[2].Value)
                });
            }
            
            context.Cards = await GetCardFromFilesFromItemsCollection(deck);
            return context.Cards;
        }
    }
}
