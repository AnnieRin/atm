using Project.Application.Auth.Common;
using Project.Application.Common.Models;
using Project.Application.Services.Accounts.Models;
using Project.Application.Services.Users.Models;
namespace Project.Application.Services.Users.Contracts;
public interface IUserService : ILoginService
{
    Task UserRegistration(UserModel userRegistartion);
    Task<AccountModel> GetAccountByPersonalN(string personalN, UserInfo userInfo);
}
