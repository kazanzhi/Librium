using Librium.Domain.Libraries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Librium.Persistence.Configurations;

public class UserLibraryConfiguration : IEntityTypeConfiguration<UserLibrary>
{
    public void Configure(EntityTypeBuilder<UserLibrary> builder)
    {
        builder.HasKey(l => l.UserId);

        builder.HasMany(l => l.Books)
               .WithOne()
               .HasForeignKey(lb => lb.UserLibraryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
