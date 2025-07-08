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
            .WithName("Category");

        group.MapGet("/category", GetCategories)
            .WithName("GetCategory")
            .Produces<APIResponse>(200);

        group.MapGet("/category/{id:int}", GetCategoryById)
            .WithName("GetCategoryById")
            .Produces<APIResponse>(200)
            .Produces(404);

        group.MapDelete("/category/{id:int}", DeleteCategory)
            .WithName("DeleteCategory")
            .Produces<APIResponse>(200)
            .Produces(404);

        group.MapPost("/category", CreateCategory)
            .WithName("PostCategory")
            .Accepts<CategoryCreateDto>("application/json")
            .Produces<APIResponse>(201)
            .Produces(404);
    }

    private static async Task<IResult> GetCategories(ICategoryRepository categoryRepository)
    {
        var categories = await categoryRepository.GetAllAsync().ConfigureAwait(false);
        
        return TypedResults.Ok(APIResponse.CreateSuccess(categories, HttpStatusCode.OK));
    }

    private static async Task<IResult> GetCategoryById(ICategoryRepository categoryRepository, int id)
    {
        var category = await categoryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (category == null)
        {
            return TypedResults.NotFound(APIResponse.CreateError(HttpStatusCode.NotFound, "Category not found"));
        }
        
        return TypedResults.Ok(APIResponse.CreateSuccess(category, HttpStatusCode.OK));
    }

    private static async Task<IResult> DeleteCategory(ICategoryRepository categoryRepository, int id)
    {
        var category = await categoryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (category == null)
        {
            return TypedResults.NotFound(APIResponse.CreateError(HttpStatusCode.NotFound, "Category not found"));
        }
        
        await categoryRepository.RemoveAsync(category).ConfigureAwait(false);
        await categoryRepository.SaveChangesAsync().ConfigureAwait(false);
        
        return TypedResults.Ok(APIResponse.CreateSuccess(HttpStatusCode.NoContent));
    }

    private static async Task<IResult> CreateCategory(ICategoryRepository categoryRepository, IMapper mapper,
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
}