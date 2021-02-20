using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Persistence.Models;

namespace Persistence.Contracts {
    public interface IDeckedBuilderRepository : IAsyncRepository<Card> {
        ValueTask<IEnumerable<Card>> GetCardsByName(string name);
    }
}
