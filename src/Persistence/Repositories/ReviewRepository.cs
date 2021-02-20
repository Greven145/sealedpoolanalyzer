using Application.Contracts.IPersistence;
using Domain.Entities;

namespace Persistence.Repositories {
    public class ReviewRepository : BaseRepository<MagicCardReview, AnalyzerContext>, IReviewRepository {
        public ReviewRepository(AnalyzerContext dbContext) : base(dbContext) {
        }
    }
}
