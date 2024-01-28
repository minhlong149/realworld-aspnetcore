using Application.DTOs;
using Application.Users.Commands.Create;
using Application.Users.Commands.Update;
using Application.Users.Queries.GetUser;
using Core.Services;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender, IUser currentUser) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Authentication(GetUserRequest request)
    {
        var user = await sender.Send(request);
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
    public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserRequest request)
    {
        var user = await sender.Send(request with { Id = currentUser.Id });
        return user;
    }
}
