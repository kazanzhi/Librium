using Librium.Application;
using Librium.Persistence;
using Librium.Persistence.Identity;

namespace Librium.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddApplication();
        builder.Services.AddPersistence(builder.Configuration);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            await IdentitySeed.SeedRolesAsync(scope.ServiceProvider);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
