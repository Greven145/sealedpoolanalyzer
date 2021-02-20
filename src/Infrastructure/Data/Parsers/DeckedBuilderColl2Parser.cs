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
using Infrastructure.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Persistence.Contracts;
using YamlDotNet.Serialization;

namespace Infrastructure.Data.Parsers
{
    public class DeckedBuilderColl2Parser : DeckParser<FileParserContext>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IDb2ScryfallRepository _db2ScryfallRepository;
        private const string _simplificationPattern = @"- (.*)";
        private const string _replacement = "$1";

        public DeckedBuilderColl2Parser(ICardRepository cardRepository, IDb2ScryfallRepository db2ScryfallRepository) {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _db2ScryfallRepository = db2ScryfallRepository ?? throw new ArgumentNullException(nameof(db2ScryfallRepository));
        }

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
            var cards = await _cardRepository.ListAllAsync();
            var relationships = await _db2ScryfallRepository.ListAllAsync();

            var cardsFromFile = (
                from colCard in collection
                join rel in relationships on colCard.id equals rel.Id
                join card in cards on rel.Scryfallid equals card.Id
                select new CardFromFile
                {
                    MultiverseId = colCard.id,
                    Name = card.Name,
                    Quantity = colCard.r
                }).ToList();

            var tempList = new List<CardFromFile>();
            foreach (var duplicates in cardsFromFile.Where(s => s.Quantity > 1)) {
                var totalQuantity = duplicates.Quantity;
                duplicates.Quantity = 1;

                for (var x = 1; x < totalQuantity; x++) {
                    tempList.Add(duplicates.ShallowCopy());
                }
            }
            
            cardsFromFile.AddRange(tempList);
            context.Cards = cardsFromFile;

            return context.Cards;
        }

        private class CollectionV2 {
            public object doc { get; set; }
            public Item[] items { get; set; }
            public int version { get; set; }

            public static CollectionV2 FromYaml(string yaml) {
                try {
                    var deserializer = new DeserializerBuilder().Build();
                    return deserializer.Deserialize<CollectionV2>(yaml);
                } catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private class Item {
            public int id { get; set; }
            public int r { get; set; }
        }
    }
}
