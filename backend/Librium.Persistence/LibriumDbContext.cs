using Librium.Domain.Books;
using Librium.Domain.Categories;
using Librium.Domain.Libraries;
using Librium.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Librium.Persistence;

public class LibriumDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>
{
    public LibriumDbContext(DbContextOptions<LibriumDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(LibriumDbContext).Assembly);
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<UserLibrary> UserLibraries { get; set; }
    public DbSet<LibraryBook> LibraryBooks { get; set; }
    public DbSet<Category> Categories { get; set; }
}
