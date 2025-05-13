using Domain.Entitys.Common;
using System.Linq.Expressions;

namespace Domain.Interfaces.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(int id,Func<IQueryable<T>, IQueryable<T>> includes = null);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}
