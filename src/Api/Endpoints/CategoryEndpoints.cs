namespace Api.Endpoints;

public static class CategoryEndpoints
{
    public static void ConfigureCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/category")
            .WithName("Category")
            .WithDisplayName("Category")
            .WithDescription("Categories used for expense organization.");

        group.MapGet("/category", GetCategories)
            .WithName("GetCategory")
            .WithDisplayName("Get category")
            .WithDescription("Categories used for expense organization.");
    }

    private static Task GetCategories(HttpContext context)
    {
        throw new NotImplementedException();
    }
}