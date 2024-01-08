using Project.Application.Services.Logs.Contracts;
using Project.Application.Common.Models;
using Project.Application.Repository;
using Project.Application.UnitOfWork;
using Project.Domain.Logs;
using Project.Application.Exceptions;
using System.Net;
using Project.Domain.Users;

namespace Project.Application.Services.Logs;
public class AuditLogService : IAuditLogService
{
    private readonly IRepository<AuditLog> _auditLogRepo;
    private readonly IUnitOfWork<AuditLog> _uniUnitOfWork;

    public AuditLogService(IRepository<AuditLog> auditLogRepo, IUnitOfWork<AuditLog> unitOfWork)
    {
        _auditLogRepo = auditLogRepo;
        _uniUnitOfWork = unitOfWork;
    }

    public async Task LogExceptionAsync(Exception exception, User user, string serviceName)
    {
        var (status, message) = DetermineExceptionDetails(exception);
        var auditLog = new AuditLog
        {
            Timestamp = DateTime.Now,
            PersonalN = user.PersonalN,
            Action = "Exception",
            Description = message,
            StatusCode = (int)status,
            ServiceName = serviceName
        };

        await _auditLogRepo.AddAsync(auditLog);
        await _uniUnitOfWork.CommitAsync();
    }

    private (HttpStatusCode, string) DetermineExceptionDetails(Exception exception)
    {
        switch (exception)
        {
            case BadRequestException ex:
                return (HttpStatusCode.BadRequest, ex.Message);
            case NotFoundException ex:
                return (HttpStatusCode.NotFound, ex.Message);
            case ValidationException ex:
                return (HttpStatusCode.BadRequest, ex.Message);
            case UnauthorizedAccessException ex:
                return (HttpStatusCode.Unauthorized, ex.Message);
            case AlreadyExistsException ex:
                return (HttpStatusCode.Conflict, ex.Message);
            default:
                return (HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}

