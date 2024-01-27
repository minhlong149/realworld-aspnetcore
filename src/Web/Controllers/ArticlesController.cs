using Application.DTOs;
using Application.Features.Articles;
using Core.Services;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ArticlesController(ISender sender, IUser currentUser) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ArticleDto>> CreateArticle(CreateArticle request)
    {
        var article = await sender.Send(request with { AuthorId = currentUser.Id });
        return article;
    }
}
