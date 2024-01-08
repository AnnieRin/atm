namespace Project.Application.Auth.Models;
public class AuthenticateResponse
{
    public string PersonalN { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }

    public AuthenticateResponse(SystemUserModel user)
    {
        PersonalN = user.PersonalN;
        UserName = user.UserName;
        Token = user.Token;
    }
}
