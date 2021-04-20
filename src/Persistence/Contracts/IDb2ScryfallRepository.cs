using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Persistence.Models;

namespace Persistence.Contracts {
    public interface IDb2ScryfallRepository  : IAsyncRepository<DeckedBuilderToScryfallRelationship> {
        Task AddRangeAsync(IEnumerable<DeckedBuilderToScryfallRelationship> entity, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> CountOfSetAsync(string setName, CancellationToken cancellationToken);
    }
}
