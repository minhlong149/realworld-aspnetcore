using Application.DTOs;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Application.Articles.Commands.Create;

public class CreateArticleHandler(
    ISlugGenerator slugGenerator,
    IUserRepository userRepository,
    IArticleRepository articleRepository,
    IMapper mapper
) : IRequestHandler<CreateArticleRequest, ArticleDto>
{
    public async Task<ArticleDto> Handle(CreateArticleRequest request, CancellationToken cancellationToken)
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

