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

        builder.ApplyConfigurationsFromAssembly(typeof(LibriumDbContext).Assembly);
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<UserBook> UserBooks { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
}
