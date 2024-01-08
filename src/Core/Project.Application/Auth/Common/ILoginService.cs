using Project.Application.Auth.Models;

namespace Project.Application.Auth.Common;
public interface ILoginService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
}
