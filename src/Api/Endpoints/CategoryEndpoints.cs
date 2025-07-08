using Api.Models;

namespace Api.Endpoints;

public static class CategoryEndpoints
{
    public static void ConfigureCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api")
            .WithName("Category")
            .WithDisplayName("Category")
            .WithDescription("Categories used for expense organization.");

        group.MapGet("/category", GetCategories)
            .WithName("GetCategory")
            .WithDisplayName("Get category")
            .WithDescription("Categories used for expense organization.");
        
        group.MapGet("/category/{id:int}", GetCategoryById)
            .WithName("GetCategoryById")
            .WithDisplayName("Get category by id")
            .WithDescription("Get a single category.");
        
        group.MapDelete("/category/{id:int}", DeleteCategory)
            .WithName("DeleteCategory")
            .WithDisplayName("Delete category")
            .WithDescription("Delete a single category.");
        
        group.MapPost("/category", PostCategory)
            .WithName("PostCategory")
            .WithDisplayName("Post category")
            .WithDescription("Post a new category.");
    }

    private static Task PostCategory(HttpContext context, Category category)
    {
        throw new NotImplementedException();
    }

    private static Task DeleteCategory(HttpContext context, int id)
    {
        throw new NotImplementedException();
    }

    private static Task GetCategoryById(HttpContext context, int id)
    {
        throw new NotImplementedException();
    }

    private static Task GetCategories(HttpContext context)
    {
        throw new NotImplementedException();
    }
}