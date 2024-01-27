using Core.Constants;
using Core.Services;
using Slugify;

namespace Infrastructure.Services;

public class SlugGenerator : ISlugGenerator
{
    public string GenerateSlug(string text)
    {
        var slug = new SlugHelper().GenerateSlug(text);
        return slug.Length > ArticleConstants.SlugMaxLength ? slug[..slug.LastIndexOf('-')] : slug;
    }
}
