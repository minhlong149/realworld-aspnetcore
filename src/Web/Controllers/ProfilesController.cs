using Application.DTOs;
using Application.Features.Users;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfilesController(ISender sender) : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<ActionResult<ProfileDto>> GetProfile(string username)
    {
        var profile = await sender.Send(new GetProfile(username));
        return profile;
    }
}
