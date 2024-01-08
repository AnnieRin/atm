using Project.Application.Common.Models;
using Project.Application.Services.Accounts.Models;
using Project.Domain.Accounts;
using Project.Domain.Users;

namespace Project.Application.Services.Accounts.Contracts;
public interface IAccountService
{
    Task<Account> CreateAccount(Guid userNumber, string personalN);
    Task UpdateAsync(Account account, string personalN);
    Task<AccountModel> GetAccount(User user);
    Task<Account> GetAccountForTransaction(string accountNumber, User user);
}
