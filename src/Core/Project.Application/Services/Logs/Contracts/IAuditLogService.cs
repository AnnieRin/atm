using Project.Application.Common.Models;
using Project.Domain.Users;

namespace Project.Application.Services.Logs.Contracts;
public interface IAuditLogService
{
    Task LogExceptionAsync(Exception exception, User user, string controllerName);    
}
