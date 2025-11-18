using Librium.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace Librium.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
