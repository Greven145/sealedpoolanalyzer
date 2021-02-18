﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IPersistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories {
    public class BaseRepository<T> : IAsyncRepository<T> where T : class {
        protected readonly AnalyzerContext _dbContext;

        public BaseRepository(AnalyzerContext dbContext) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual async ValueTask<T> GetByIdAsync(Guid id) {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async ValueTask<IReadOnlyList<T>> ListAllAsync() {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async ValueTask<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size) {
            return await _dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async ValueTask<T> AddAsync(T entity) {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity) {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity) {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}