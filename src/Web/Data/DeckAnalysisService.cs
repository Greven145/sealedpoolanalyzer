using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Application.Features.DataAnalysis;
using Application.Models;
using Domain.Entities;
using Infrastructure.Models.tmp.CardPool;

namespace Web.Data {
    /// <summary>
    ///     TODO: This needs to be not here.
    /// </summary>
    public class DeckAnalysisService {
        private readonly ISetRepository _repository;

        public DeckAnalysisService(ISetRepository repository) {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async ValueTask<IEnumerable<MagicCard>> GetCardsForPool(IEnumerable<SealedCards> sealedCards) {
            //TODO: Hard coded set name
            var set = await _repository.GetSetByShortName("KHM").ConfigureAwait(false);
            var cards = set.Cards.Join(sealedCards, setCard => setCard.Name, sealedCard => sealedCard.Card,
                (setCard, sealedCard) => new {setCard, sealedCard}).ToList();

            var tempList = new List<MagicCard>();

            foreach (var duplicates in cards.Where(s => s.sealedCard.TotalQty > 1)) {
                for (var x = 1; x < duplicates.sealedCard.TotalQty; x++) {
                    tempList.Add(duplicates.setCard);
                }
            }

            tempList.AddRange(cards.Select(c => c.setCard));
            return tempList;
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
