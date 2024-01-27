using Core.Entities;
using Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ArticleRepository(ConduitContext context) : Repository<ArticleEntity>(context), IArticleRepository
{
}
