using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Application.Features.DataAnalysis;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data.Parsers;
using Infrastructure.Data.Parsers.Pipeline;
using Infrastructure.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Web.Data {
    /// <summary>
    ///     TODO: This needs to be not here.
    /// </summary>
    public class DeckAnalysisService {
        private readonly IDeckParser<FileParserContext> _deckParser;
        private readonly ILogger<DeckAnalysisService> _logger;
        private readonly ISetRepository _repository;

        public IEnumerable<(string Name, decimal Rating)> DeckAndRatings;
        public IEnumerable<(string Name, decimal Rating)> PoolAndRatings;
        public IEnumerable<(string Color, string Rating)> ColorRatings { get; set; }
        public IEnumerable<ConsiderCards> InColorCards { get; set; }
        public IEnumerable<ConsiderCards> SharedColorCards { get; set; }
        public IEnumerable<TribalCards> TribalData { get; set; }
        
        private IEnumerable<MagicCard> _deck;
        private IEnumerable<MagicCard> _pool;

        public DeckAnalysisService(ISetRepository repository, ILogger<DeckAnalysisService> logger,
            IDeckParser<FileParserContext> deckParser, IMapper mapper) {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _deckParser = deckParser ?? throw new ArgumentNullException(nameof(deckParser));
        }

        public async Task ProcessDeckChangeEvent(InputFileChangeEventArgs e) {
            _deck = await ParseFile(e);
            await DisplayWhatIsPossible();
        }

        public async Task ProcessPoolChangeEvent(InputFileChangeEventArgs e) {
            _pool = await ParseFile(e);
            await DisplayWhatIsPossible();
        }

        private async ValueTask<IEnumerable<MagicCard>> GetCardsForPool(IEnumerable<CardFromFile> sealedCards) {
            var sets = sealedCards.Select(x => x.Set).Distinct();

            var cards = new List<MagicCard>();
            foreach (var set in sets) {
                var setFromName = await _repository.GetSetByName(set);
                cards.AddRange(sealedCards.Select(sealedCard => 
                    setFromName.MagicCards.First(m => 
                        m.MultiverseIds.Contains(sealedCard.MultiverseId)
                    )
                ).ToList());
            }

            var cardsNotFound = sealedCards
                .Where(s => !cards.SelectMany(t => t.MultiverseIds).Contains(s.MultiverseId))
                .Select(s => s.Name);

            if (cardsNotFound.Any()) {
                _logger.LogWarning("Unable to find the following cards: {0}", cardsNotFound);
            }

            return cards;
        }


        private Task<IEnumerable<(string Name, decimal Rating)>> GetDeckAnalysis(IEnumerable<MagicCard> mergedData) {
            var basicAnalysisData = DataAnalysis.GetPoolRatings(mergedData);
            var cardsToDisplayForDeck = basicAnalysisData
                .OrderByDescending(i => i.Rating)
                .ThenBy(i => i.Name)
                .AsEnumerable();
            return Task.FromResult(cardsToDisplayForDeck);
        }

        private IEnumerable<TribalCards> GetTribalData(IEnumerable<MagicCard> pool) {
            return DataAnalysis.GetTribalAnalysis(pool);
        }

        private IEnumerable<(string Color, string Rating)> WriteColorRatings(IEnumerable<MagicCard> pool) {
            return DataAnalysis.GetColorRatings(pool);
        }

        private (IEnumerable<ConsiderCards> InColorCards, IEnumerable<ConsiderCards> SharedColorCards)
            GetGoodCardsYoureNotPlaying(IEnumerable<MagicCard> deck, IEnumerable<MagicCard> pool) {
            return DataAnalysis.GetCardsToConsider(deck, pool);
        }

        private async Task DisplayWhatIsPossible() {
            if (_deck != null) {
                DeckAndRatings = await GetDeckAnalysis(_deck);
            }

            if (_pool != null) {
                PoolAndRatings = await GetDeckAnalysis(_pool);
                TribalData = GetTribalData(_pool).OrderByDescending(t => t.TribalRating);
                ColorRatings = WriteColorRatings(_pool);
            }

            if (_deck != null && _pool != null) {
                (InColorCards, SharedColorCards) = GetGoodCardsYoureNotPlaying(_deck, _pool);
            }
        }

        private async Task<IEnumerable<MagicCard>> ParseFile(InputFileChangeEventArgs e) {
            try {
                var cards = await _deckParser.Execute(await FileParserContext.FromIBrowserFile(e.File));
                return await GetCardsForPool(cards);
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
