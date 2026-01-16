using Librium.Application.Abstractions.Services;
using Librium.Application.Services.Books;
using Librium.Application.Services.Categories;
using Librium.Application.Services.Comments;
using Librium.Application.Services.Libraries;
using Microsoft.Extensions.DependencyInjection;

namespace Librium.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IUserLibraryService, UserLibraryService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}
