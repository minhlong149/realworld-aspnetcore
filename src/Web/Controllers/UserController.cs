using Application.DTOs;
using Application.Features.Users;
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
}
