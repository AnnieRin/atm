using Project.Application.Auth.Models;
using Project.Domain.Users;

namespace Project.Application.Auth.Common;
public class LoginService : ILoginService
{
    private readonly ITokenService _tokenService;
    public LoginService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }


    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        //check user userName and password in DB
        var user = new User();
        var systemUser = new SystemUserModel();


        if (user == null) return null;

        //var token = _tokenService.BuildToken(user);

        //return new AuthenticateResponse(systemUser, token);
        return new AuthenticateResponse(new SystemUserModel());
    }
}
