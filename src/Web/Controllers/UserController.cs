using Application.DTOs;
using Application.Features.Users;
using Application.Users.Commands.Create;
using Core.Services;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Authentication(Authentication command)
    {
        var user = await sender.Send(command);
        return user;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> Registration(CreateUserRequest request)
    {
        var user = await sender.Send(request);
        return user;
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateUser(
        [FromBody] UpdateUser command,
        [FromServices] IUser currentUser
    )
    {
        var user = await sender.Send(command with { Id = currentUser.Id });
        return user;
    }
}
