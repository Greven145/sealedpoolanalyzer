using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories {
    public class CardRepository : BaseRepository<MagicCard, AnalyzerContext>, ICardRepository {
        private readonly AnalyzerContext _dbContext;

        public CardRepository(AnalyzerContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
        }

        public new async ValueTask<IReadOnlyList<MagicCard>> ListAllAsync() => 
            await _dbContext.Sets
                .Include(s => s.MagicCards)
                .ThenInclude(m => m.Set)
                .SelectMany(s => s.MagicCards).ToListAsync();
    }
}
