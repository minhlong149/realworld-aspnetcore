namespace Core.Entities;

public class ArticleEntity : BaseEntity
{
    public required string Slug { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Body { get; set; }

    public required UserEntity Author { get; set; }

    public Guid AuthorId { get; set; }
}
