using System.Net;
using Api.Endpoints.CategoryEndpoint.DTO;
using Api.Endpoints.CategoryEndpoint.Repository.Interfaces;
using Api.Models;
using AutoMapper;

namespace Api.Endpoints.CategoryEndpoint;

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
        
        group.MapPost("/category", CreateCategoryAsync)
            .WithName("PostCategory")
            .Accepts<CategoryCreateDto>("application/json")
            .Produces<APIResponse>(201)
            .Produces(400)
            .WithDisplayName("Post category")
            .WithDescription("Post a new category.");
    }

    private static async Task<IResult> CreateCategoryAsync(ICategoryRepository categoryRepository, IMapper mapper,
        CategoryCreateDto newCategory)
    {
        if (await categoryRepository.GetByNameAsync(newCategory.Name).ConfigureAwait(false) != null)
        {
            return TypedResults.BadRequest(
                APIResponse.CreateError(HttpStatusCode.BadRequest, "Coupon Name already Exists"));
        }
        
        var category = mapper.Map<Category>(newCategory);

        await categoryRepository.CreateAsync(category).ConfigureAwait(false);
        await categoryRepository.SaveChangesAsync().ConfigureAwait(false);

        var categoryDto = mapper.Map<CategoryDto>(category);

        return TypedResults.Ok(APIResponse.CreateSuccess(categoryDto, HttpStatusCode.Created));
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