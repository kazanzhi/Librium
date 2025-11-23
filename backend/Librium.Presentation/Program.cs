using Librium.Application;
using Librium.Identity;
using Librium.Persistence;
using Librium.Persistence.Identity;
using Microsoft.EntityFrameworkCore;

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
        builder.Services.AddIdentityInfrastructure();
        builder.Services.AddPersistenceInfrastructure(builder.Configuration);

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var db = services.GetRequiredService<LibriumDbContext>();
            db.Database.Migrate();

            await IdentitySeed.SeedRolesAsync(scope.ServiceProvider);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
