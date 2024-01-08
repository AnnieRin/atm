using Project.Application.Auth.Models;

namespace Project.Application.Auth.Common;
public interface ITokenService
{
    SystemUserModel BuildToken(SystemUserModel user);
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
