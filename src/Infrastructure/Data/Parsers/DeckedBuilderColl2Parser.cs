using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Infrastructure.Data.Parsers.Pipeline;
using Infrastructure.Data.Parsers.Pipeline.Bases;
using Infrastructure.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Persistence.Contracts;
using YamlDotNet.Serialization;

namespace Infrastructure.Data.Parsers
{
    public class DeckedBuilderColl2Parser : DeckedBuilderParser
    {
        private const string _simplificationPattern = @"- (.*)";
        private const string _replacement = "$1";

        public DeckedBuilderColl2Parser(ICardRepository cardRepository, IDb2ScryfallRepository db2ScryfallRepository) :
            base(cardRepository, db2ScryfallRepository) { }

        protected override async ValueTask<IEnumerable<CardFromFile>> Execute(FileParserContext context,
            Func<FileParserContext, ValueTask<IEnumerable<CardFromFile>>> next)
        {
            //TODO: Known defect: Can't catch showcase cards.
            var fileExtension = Path.GetExtension(context.Name);

            if (string.IsNullOrEmpty(fileExtension) ||
                !fileExtension.Equals(".coll2", StringComparison.InvariantCultureIgnoreCase))
            {
                return await next(context);
            }

            var fileContent = Encoding.ASCII.GetString(context.Content);

            var collection = CollectionV2.FromYaml(Regex.Replace(fileContent,_simplificationPattern,_replacement)).items;
            context.Cards = await GetCardFromFilesFromItemsCollection(collection);
            return context.Cards;
        }
    }
}
