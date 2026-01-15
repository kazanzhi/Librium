using Librium.Domain.Libraries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Librium.Persistence.Configurations;

public class LibraryBookConfiguration : IEntityTypeConfiguration<LibraryBook>
{
    public void Configure(EntityTypeBuilder<LibraryBook> builder)
    {
        builder.HasKey(x => new { x.UserLibraryId, x.BookId });

        builder.Property(lb => lb.BookId)
               .IsRequired();

        builder.Property(lb => lb.UserLibraryId)
               .IsRequired();
    }
}
