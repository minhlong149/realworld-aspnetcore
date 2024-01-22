using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ArticleRepository(ConduitContext context) : Repository<ArticleEntity>(context), IArticleRepository
{
}
