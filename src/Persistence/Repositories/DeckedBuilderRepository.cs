using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;
using Persistence.Models;

namespace Persistence.Repositories {
    public class DeckedBuilderRepository : BaseRepository<Card,DeckedBuilderContext>, IDeckedBuilderRepository {
        public DeckedBuilderRepository(DeckedBuilderContext dbContext) : base(dbContext) {
        }

        public async ValueTask<IEnumerable<Card>> GetCardsByName(string name) {
            return await _dbContext.Cards.Where(c => c.Cardset == name).ToListAsync();
        }
    }
}
