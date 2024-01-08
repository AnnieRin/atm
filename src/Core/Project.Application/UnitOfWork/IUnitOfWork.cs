using Project.Application.Repository;
using Project.Domain.Common;

namespace Project.Application.UnitOfWork;
public interface IUnitOfWork<T> where T: BaseEntity
{
    IRepository<T> Repository { get; }
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
