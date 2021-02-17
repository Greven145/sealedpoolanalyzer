using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Application.Features.DataAnalysis;
using Application.Models;
using Domain.Entities;
using Infrastructure.Models.tmp.CardPool;
using Microsoft.Extensions.Logging;

namespace Web.Data {
    /// <summary>
    ///     TODO: This needs to be not here.
    /// </summary>
    public class DeckAnalysisService {
        private readonly ILogger<DeckAnalysisService> _logger;
        private readonly ISetRepository _repository;

        public DeckAnalysisService(ISetRepository repository, ILogger<DeckAnalysisService> logger) {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<IEnumerable<MagicCard>> GetCardsForPool(IEnumerable<SealedCards> sealedCards) {
            //TODO: Hard coded set name
            var set = await _repository.GetSetByShortName("khm");
            var cards = set.MagicCards.Join(sealedCards, setCard => setCard.Name, sealedCard => sealedCard.Card,
                (setCard, sealedCard) => setCard).ToList();

            var cardsNotfound = sealedCards
                .Where(s => !cards.Select(t => t.Name).Contains(s.Card))
                .Select(s => s.Card);
            
            if (cardsNotfound.Any()) {
                _logger.LogWarning("Unable to find the following cards: {0}", cardsNotfound);
            }

            return cards;
        }


        public Task<IEnumerable<(string Name, decimal Rating)>> GetDeckAnalysis(IEnumerable<MagicCard> mergedData) {
            var basicAnalysisData = DataAnalysis.GetPoolRatings(mergedData);
            var cardsToDisplayForDeck = basicAnalysisData
                .OrderByDescending(i => i.Rating)
                .ThenBy(i => i.Name)
                .AsEnumerable();
            return Task.FromResult(cardsToDisplayForDeck);
        }

        public IEnumerable<TribalCards> GetTribalData(IEnumerable<MagicCard> pool) {
            return DataAnalysis.GetTribalAnalysis(pool);
        }

        public IEnumerable<(string Color, string Rating)> WriteColorRatings(IEnumerable<MagicCard> pool) {
            return DataAnalysis.GetColorRatings(pool);
        }

        public (IEnumerable<ConsiderCards> InColorCards, IEnumerable<ConsiderCards> SharedColorCards)
            GetGoodCardsYoureNotPlaying(IEnumerable<MagicCard> deck, IEnumerable<MagicCard> pool) {
            return DataAnalysis.GetCardsToConsider(deck, pool);
        }
    }
}
