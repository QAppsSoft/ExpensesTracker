using Api.Endpoints;
using Scalar.AspNetCore;

namespace Api;

public class Program
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

        var app = builder.Build();

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