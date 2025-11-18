using System.Linq.Expressions;

using ProgressSoft.Domain.Entities;

namespace ProgressSoft.Domain.Interfaces.Infrastructure.IRepositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<PaginationResult<T>> GetAllAsync(
        int? pageNumber,
        int? pageSize,
        Expression<Func<T, bool>>? filter);
    Task AddBulkAsync(List<T> entities);
    Task<T> AddAsync(T entity);
    T? Update(T entity);
    Task DeleteAsync(Guid id);
}
