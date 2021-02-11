using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic;
using Logic.models;

namespace Web.Data
{
    public class DeckAnalysisService
    {
        public Task<IEnumerable<(string Name, double Rating)>> GetDeckAnalysis(IEnumerable<MergedData> mergedData) {
            var basicAnalysisData = DataAnalysis.GetPoolRatings(mergedData);
            var cardsToDisplayForDeck = basicAnalysisData
                .OrderByDescending(i => i.Rating)
                .ThenBy(i => i.Name)
                .AsEnumerable();
            return Task.FromResult(cardsToDisplayForDeck);
        }

        public IEnumerable<TribalCards> GetTribalData(IEnumerable<MergedData> pool) => DataAnalysis.GetTribalAnalysis(pool);

        public IEnumerable<(string Color, string Rating)> WriteColorRatings(IEnumerable<MergedData> pool) => DataAnalysis.GetColorRatings(pool);

        public (IEnumerable<ConsiderCards> InColorCards, IEnumerable<ConsiderCards> SharedColorCards) GetGoodCardsYoureNotPlaying(IEnumerable<MergedData> deck, IEnumerable<MergedData> pool) => DataAnalysis.GetCardsToConsider(deck, pool);
    }
}
