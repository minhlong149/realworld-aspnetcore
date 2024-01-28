using Application.DTOs;

namespace Application.Articles.Commands.Create;

public record CreateArticleRequest(Guid AuthorId) : IRequest<ArticleDto>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Body { get; init; }
}
