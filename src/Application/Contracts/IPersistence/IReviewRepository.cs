using Domain.Entities;

namespace Application.Contracts.IPersistence {
    public interface IReviewRepository : IAsyncRepository<MagicCardReview> {
    }
}
