using Microsoft.AspNetCore.Mvc.Filters;
using Project.Api.Controllers;
using Project.Application.Common.Models;
using System.Security.Claims;

namespace Project.Api.Filters;

public class UserFilterAttribute : Attribute, IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var c = context.Controller as ApiControllerBase;
        c.UserInfo = new UserInfo()
        {
            PersonalN = c.User.FindFirstValue(ClaimTypes.NameIdentifier),
            UserName = c.User.FindFirstValue(ClaimTypes.Name)
        };
    }
}
