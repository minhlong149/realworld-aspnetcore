using Application.DTOs;
using Application.Features.Users;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<UserDto>> Register(Registration command)
    {
        var user = await sender.Send(command);
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
