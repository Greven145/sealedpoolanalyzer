using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts.IPersistence {
    public interface IAsyncRepository<T> where T : class {
        ValueTask<T> GetByIdAsync(Guid id);
        ValueTask<IReadOnlyList<T>> ListAllAsync();
        ValueTask<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        ValueTask<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size);
    }
}
