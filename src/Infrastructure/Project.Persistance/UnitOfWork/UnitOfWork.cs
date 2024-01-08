using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Repository;
using Project.Application.UnitOfWork;
using Project.Domain.Common;
using Project.Persistance.Data;

namespace Project.Persistance.UnitOfWork;
public class UnitOfWork<T> : IUnitOfWork<T>, IDisposable where T : BaseEntity
{
    private readonly AppDbContext _dbContext;
    private readonly IRepository<T> _baseRepository;
    private IDbContextTransaction _currentTransaction;

    public IRepository<T> Repository { get { return _baseRepository; } }

    public UnitOfWork(AppDbContext dbContext, IRepository<T> baseRepository)
    {
        _dbContext = dbContext;
        _baseRepository = baseRepository;
    }

    public async Task BeginTransactionAsync()
    {
        _currentTransaction ??= await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _dbContext.Dispose();
    }
}
