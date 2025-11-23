using Librium.Domain.Repositories;
using Librium.Domain.Users.Models;
using Librium.Domain.Users.Repositories;
using Librium.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Librium.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<LibriumDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<LibriumDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
