using Core.Entities;

namespace Application.DTOs;

public class ArticleDto
{
    public string? Slug { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Body { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public AuthorDto? Author { get; init; }

    public class ArticleProfile : Profile
    {
        public ArticleProfile() { CreateMap<ArticleEntity, ArticleDto>(); }
    }
}
