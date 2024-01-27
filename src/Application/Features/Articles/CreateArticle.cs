using Application.DTOs;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;

namespace Application.Features.Articles;

public record CreateArticle(Guid AuthorId) : IRequest<ArticleDto>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Body { get; init; }
}

public class CreateArticleValidator : AbstractValidator<CreateArticle>
{
    public CreateArticleValidator()
    {
        RuleFor(article => article.Title)
            .MaximumLength(ArticleConstants.TitleMaxLength).WithMessage(ArticleConstants.TitleLengthExceeded)
            .NotEmpty().WithMessage(ArticleConstants.TitleRequired);

        RuleFor(article => article.Description)
            .MaximumLength(ArticleConstants.DescriptionMaxLength).WithMessage(ArticleConstants.DescriptionLengthExceeded)
            .NotEmpty().WithMessage(ArticleConstants.DescriptionRequired);

        RuleFor(article => article.Body)
            .MaximumLength(ArticleConstants.BodyMaxLength).WithMessage(ArticleConstants.BodyLengthExceeded)
            .NotEmpty().WithMessage(ArticleConstants.BodyRequired);

        RuleFor(article => article.AuthorId)
            .NotEmpty().WithMessage(ArticleConstants.AuthorRequired);
    }
}

public class CreateArticleHandler(
    ISlugGenerator slugGenerator,
    IUserRepository userRepository,
    IArticleRepository articleRepository,
    IMapper mapper
) : IRequestHandler<CreateArticle, ArticleDto>
{
    public async Task<ArticleDto> Handle(CreateArticle request, CancellationToken cancellationToken)
    {
        var author = await userRepository.GetByIdAsync(request.AuthorId);

        if (author is null)
        {
            throw new InvalidCredentialsException(ArticleConstants.AuthorNotFound);
        }

        var newArticle = new ArticleEntity
        {
            Slug = slugGenerator.GenerateSlug(request.Title),
            Title = request.Title,
            Description = request.Description,
            Body = request.Body,
            Author = author,
        };

        await articleRepository.CreateAsync(newArticle, cancellationToken);

        return mapper.Map<ArticleDto>(newArticle);
    }
}
