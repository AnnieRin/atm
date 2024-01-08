using Project.Domain.Common;
using System.Linq.Expressions;

namespace Project.Application.Repository;
public interface IRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync(string relatedProperties = null);
    Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate, string relatedProperties = null);
    Task<T> FindAsync(Expression<Func<T, bool>> predicate, string relatedProperties = null);
    Task<T> GetByIdAsync(int id, string relatedProperties = null);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
