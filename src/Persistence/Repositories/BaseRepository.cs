using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories {
    public abstract class BaseRepository<TSet, TContext> : IAsyncRepository<TSet> where TSet : class where TContext : DbContext {
        protected readonly TContext _dbContext;

        protected BaseRepository(TContext dbContext) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual async ValueTask<TSet> GetByIdAsync(Guid id) {
            return await _dbContext.Set<TSet>().FindAsync(id);
        }

        public async ValueTask<IReadOnlyList<TSet>> ListAllAsync() {
            return await _dbContext.Set<TSet>().ToListAsync();
        }

        public virtual async ValueTask<IReadOnlyList<TSet>> GetPagedReponseAsync(int page, int size) {
            return await _dbContext.Set<TSet>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async ValueTask<TSet> AddAsync(TSet entity) {
            await _dbContext.Set<TSet>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(TSet entity) {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TSet entity) {
            _dbContext.Set<TSet>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
