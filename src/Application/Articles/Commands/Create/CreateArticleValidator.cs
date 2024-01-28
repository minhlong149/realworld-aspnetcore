using Core.Constants;

namespace Application.Articles.Commands.Create;

public class CreateArticleValidator : AbstractValidator<CreateArticleRequest>
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
