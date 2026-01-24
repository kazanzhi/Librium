using Librium.Application.Abstractions.Services;
using Librium.Application.Services.Comments;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Librium.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommentService, CommentService>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
