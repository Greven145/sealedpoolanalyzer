using System;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories {
    public class SetRepository : BaseRepository<Set>, ISetRepository {
        public SetRepository(AnalyzerContext dbContext) : base(dbContext) {
        }

        public async ValueTask<Set> GetSetByName(string name) {
            return await _dbContext.Sets
                .Include(s => s.MagicCards)
                .ThenInclude(c => c.Review)
                .FirstAsync(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public async ValueTask<Set> GetSetByShortName(string shortName) {
            return await _dbContext.Sets
                .Include(s => s.MagicCards)
                .ThenInclude(c => c.Review)
                .FirstAsync(s => s.Code == shortName);
        }
    }
}
