using Librium.Application.Interfaces;
using Librium.Application.Services;
using Librium.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Librium.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookCategoryService, BookCategoryService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IUserBookService, UserBookService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
