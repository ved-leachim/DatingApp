using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    var resultContext = await next();

    // Probably unnecessary because the filter will be used on Controllers where authentication is needed
    if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

    var username = resultContext.HttpContext.User.GetUsername();
    var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
    var user = await repo.GetUserByUsernameAsync(username);
    user.LastActive = DateTime.UtcNow;
    await repo.SaveAllAsync();
  }
}
