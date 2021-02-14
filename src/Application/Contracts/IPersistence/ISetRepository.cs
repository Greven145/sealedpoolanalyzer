using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Contracts.IPersistence {
    public interface ISetRepository : IAsyncRepository<Set> {
        ValueTask<Set> GetSetByName(string name);
        ValueTask<Set> GetSetByShortName(string shortName);
    }
}
