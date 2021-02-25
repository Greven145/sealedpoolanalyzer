using Application.Contracts.IPersistence;
using Domain.Entities;

namespace Persistence.Repositories {
    public class CardRepository : BaseRepository<MagicCard, AnalyzerContext>, ICardRepository {
        public CardRepository(AnalyzerContext dbContext) : base(dbContext) {
        }
    }
}
