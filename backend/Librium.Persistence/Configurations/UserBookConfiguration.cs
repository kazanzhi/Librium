using Librium.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Librium.Persistence.Configurations;

public class UserBookConfiguration : IEntityTypeConfiguration<UserBook>
{
    public void Configure(EntityTypeBuilder<UserBook> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne(ub => ub.AppUser)
            .WithMany(u => u.UserBooks)
            .HasForeignKey(ub => ub.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ub => ub.Book)
            .WithMany(b => b.UserBooks)
            .HasForeignKey(ub => ub.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}