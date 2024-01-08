using Microsoft.AspNetCore.Mvc;
using Project.Api.Filters;
using Project.Application.Common.Models;

namespace Project.Api.Controllers;
[ApiController]
[UserFilter]
public class ApiControllerBase : ControllerBase
{
    public UserInfo UserInfo { get; set; }
}
