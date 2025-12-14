using Librium.Domain.Books.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Librium.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired();

        builder.Property(b => b.Author)
            .IsRequired();

        builder.Property(b => b.Content)
            .IsRequired();

        builder.Property(b => b.PublishedYear)
            .IsRequired();

        builder.HasOne(b => b.BookCategories)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}