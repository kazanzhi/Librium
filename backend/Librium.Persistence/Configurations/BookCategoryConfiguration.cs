using Librium.Domain.Books.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Librium.Persistence.Configurations;

public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(20);
    }
}
