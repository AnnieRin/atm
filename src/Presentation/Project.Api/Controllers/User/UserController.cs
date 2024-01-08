using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Auth.Models;
using Project.Application.Services.Users.Contracts;
using Project.Application.Services.Users.Models;

namespace Project.Api.Controllers.User;
[ApiController]
[Route("[controller]")]
public class UserController : ApiControllerBase
{

    public readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Registration")]
    public async Task<ActionResult> Registration([FromBody] UserModel userModel)
    {
        await _userService.UserRegistration(userModel);
        return Ok();
    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] AuthenticateRequest model)
    {
        var result = await _userService.Authenticate(model);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("GetUserAccount/{personalN}")]
    public async Task<ActionResult> GetUserAccount([FromRoute] string personalN)
    {
        var account = await _userService.GetAccountByPersonalN(personalN, UserInfo);
        return Ok(account);
    }
}
