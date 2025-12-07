using Librium.Application.Services;
using Librium.Domain.Books.Services;
using Librium.Domain.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Librium.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookCategoryService, BookCategoryService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
