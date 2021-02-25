using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;
using Persistence.Models;

namespace Persistence.Repositories {
    public class Db2ScrylfallRepository : BaseRepository<DeckedBuilderToScryfallRelationship, AnalyzerContext>, IDb2ScryfallRepository {
        public Db2ScrylfallRepository(AnalyzerContext dbContext) : base(dbContext) {
        }

        public async Task AddRangeAsync(IEnumerable<DeckedBuilderToScryfallRelationship> entity, CancellationToken cancellationToken = default) {
            await _dbContext.DB2Scryfall.AddRangeAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken) => _dbContext.DB2Scryfall.CountAsync(cancellationToken);
    }
}
