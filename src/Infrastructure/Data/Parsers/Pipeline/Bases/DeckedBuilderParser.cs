using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Infrastructure.Models;
using Persistence.Contracts;
using YamlDotNet.Serialization;

namespace Infrastructure.Data.Parsers.Pipeline.Bases {
    public abstract class DeckedBuilderParser : DeckParser<FileParserContext>{
        private readonly ICardRepository _cardRepository;
        private readonly IDb2ScryfallRepository _db2ScryfallRepository;

        protected DeckedBuilderParser(ICardRepository cardRepository, IDb2ScryfallRepository db2ScryfallRepository) {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _db2ScryfallRepository = db2ScryfallRepository ?? throw new ArgumentNullException(nameof(db2ScryfallRepository));
        }

        protected IEnumerable<CardFromFile> DuplicateByQuantity(IEnumerable<CardFromFile> cardsFromFiles) {
            var tempList = new List<CardFromFile>();
            foreach (var duplicates in cardsFromFiles.Where(s => s.Quantity > 1)) {
                var totalQuantity = duplicates.Quantity;
                duplicates.Quantity = 1;

                for (var x = 1; x < totalQuantity; x++) {
                    tempList.Add(duplicates.ShallowCopy());
                }
            }

            return tempList;
        }
        
        protected virtual async Task<IEnumerable<CardFromFile>> GetCardFromFilesFromItemsCollection(IEnumerable<Item> deck) {
            var cards = await _cardRepository.ListAllAsync();
            var relationships = await _db2ScryfallRepository.ListAllAsync();

            var cardsFromFile = (
                from decCard in deck
                join rel in relationships on decCard.id equals rel.Id
                join card in cards on rel.Scryfallid equals card.Id
                select new CardFromFile {
                    MultiverseId = card.MultiverseIds.First(),
                    Name = card.Name,
                    Quantity = decCard.r
                }).ToList();

            cardsFromFile.AddRange(DuplicateByQuantity(cardsFromFile));
            return cardsFromFile;
        }
        
        protected class CollectionV2 {
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
        protected class Item {
            public int id { get; set; }
            public int r { get; set; }
        }
    }
}
