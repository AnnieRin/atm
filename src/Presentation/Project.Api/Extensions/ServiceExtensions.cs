using Project.Application.Auth.Common;
using Project.Application.Repository;
using Project.Application.Services.Accounts;
using Project.Application.Services.Accounts.Contracts;
using Project.Application.Services.Logs;
using Project.Application.Services.Logs.Contracts;
using Project.Application.Services.Transactions;
using Project.Application.Services.Transactions.Contracts;
using Project.Application.Services.Users;
using Project.Application.Services.Users.Contracts;
using Project.Persistance.Repository;

namespace Project.Api.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
