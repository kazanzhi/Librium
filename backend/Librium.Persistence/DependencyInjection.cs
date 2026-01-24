using Librium.Application.Libraries.Repositories;
using Librium.Domain.Books.Repositories;
using Librium.Domain.Categories.Repositories;
using Librium.Domain.Comments.Repositories;
using Librium.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Librium.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<LibriumDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserBookRepository, UserBookRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        return services;
    }
}
