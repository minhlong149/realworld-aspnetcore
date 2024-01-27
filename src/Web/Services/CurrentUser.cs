using System.Security.Claims;
using Core.Services;

namespace Web.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor): IUser
{
    public Guid Id => Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
}
