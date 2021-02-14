using Domain.Entities;

namespace Application.Contracts.IPersistence {
    public interface ICardRepository : IAsyncRepository<MagicCard> {
    }
}
