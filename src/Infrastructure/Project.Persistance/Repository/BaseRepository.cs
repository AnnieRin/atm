using Project.Application.Repository;
using Project.Domain.Common;
using Project.Persistance.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Project.Persistance.Repository;
public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _dbContext;

    public BaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<T> AddAsync(T entity)
    {
        var query = await _dbContext.Set<T>().AddAsync(entity);
        return query.Entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> FindAsync(Expression<Func<T, bool>> predicate, string relatedProperties = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (relatedProperties != null)
            query = query.Include(relatedProperties);

        return await query.Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate, string relatedProperties = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (relatedProperties != null)
            query = query.Include(relatedProperties);

        return await query.Where(predicate).ToListAsync();
    }

    public async Task<List<T>> GetAllAsync(string relatedProperties = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (relatedProperties != null) query = query.Include(relatedProperties);

        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id, string relatedProperties = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (relatedProperties != null)
            query = query.Include(relatedProperties);

        return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }
}
