using Librium.Domain.Books.Models;
using Librium.Domain.Entities.Books;
using Librium.Domain.Users.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence;

public class LibriumDbContext : IdentityDbContext<AppUser>
{
    public LibriumDbContext(DbContextOptions<LibriumDbContext> options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserBook>()
            .HasOne(ub => ub.AppUser)
            .WithMany(u => u.UserBooks)
            .HasForeignKey(ub => ub.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserBook>()
            .HasOne(ub => ub.Book)
            .WithMany(b => b.UserBooks)
            .HasForeignKey(ub => ub.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Book>()
            .HasOne(b => b.BookCategory)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<UserBook> UserBooks { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
}
