using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<ArticleEntity>
{
    public void Configure(EntityTypeBuilder<ArticleEntity> builder)
    {
        builder.HasIndex(article => article.Slug).IsUnique();

        builder.Property(article => article.Slug).IsRequired();
        builder.Property(article => article.Title).IsRequired();
        builder.Property(article => article.Description).IsRequired();
        builder.Property(article => article.Body).IsRequired();

        builder
            .HasOne(article => article.Author)
            .WithMany()
            .HasForeignKey(article => article.AuthorId)
            .IsRequired();
    }
}
