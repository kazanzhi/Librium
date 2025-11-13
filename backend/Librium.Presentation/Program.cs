
using Librium.Application.Services;
using Librium.Domain.Entities.Books;
using Librium.Domain.Interfaces;
using Librium.Domain.Repositories;
using Librium.Persistence;
using Librium.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Librium.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<LibriumDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
        builder.Services.AddScoped<IBookCategoryService, BookCategoryService>();
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IUserBookRepository, UserBookRepository>();
        builder.Services.AddScoped<IUserBookService, UserBookService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
