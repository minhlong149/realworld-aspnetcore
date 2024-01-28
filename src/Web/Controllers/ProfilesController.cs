using Application.DTOs;
using Application.Users.Queries.GetProfile;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfilesController(ISender sender) : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<ActionResult<ProfileDto>> GetProfile(string username)
    {
        var profile = await sender.Send(new GetProfileRequest(username));
        return profile;
    }
}
