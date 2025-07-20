using Api.Data.Context;
using Api.Endpoints.CategoryEndpoint;
using Api.Endpoints.CategoryEndpoint.Repository;
using Api.Endpoints.CategoryEndpoint.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        // Register the DbContext
        builder.Services.AddDbContext<ExpensesTrackerDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Register the CategoryRepository
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

        builder.Services.AddCors(options =>
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            ));

        var app = builder.Build();
        
        app.UseCors("AllowAll");
        
        // Ensure the database is created
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ExpensesTrackerDbContext>();
        dbContext.Database.EnsureCreated();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                    .WithTheme(ScalarTheme.Kepler)
                    .WithDarkModeToggle()
                    .WithClientButton();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.ConfigureCategoryEndpoints();

        app.Run();
    }
}