using System.Diagnostics.CodeAnalysis;
using Api.Data.Extensions;
using Api.Endpoints.CategoryEndpoint.Repository.Interfaces;
using Api.Models.Dto.Categories;

namespace Api.Endpoints.CategoryEndpoint;

[SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods")]
public static class CategoryEndpoints
{
    public static void ConfigureCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1")
            .WithName("Category");

        group.MapGet("/categories", GetCategories)
            .WithName("GetCategory")
            .Produces<ICollection<CategoryDto>>();

        group.MapGet("/categories/{id:int}", GetCategoryById)
            .WithName("GetCategoryById")
            .Produces<CategoryDto>()
            .Produces(404);

        group.MapDelete("/categories/{id:int}", DeleteCategory)
            .WithName("DeleteCategory")
            .Produces(204)
            .Produces(404);

        group.MapPost("/categories", CreateCategory)
            .WithName("PostCategory")
            .Accepts<CreateCategoryDto>("application/json")
            .Produces<CategoryDto>(201)
            .Produces(400);
    }

    private static async Task<IResult> GetCategories(ICategoryRepository categoryRepository)
    {
        var categories = await categoryRepository.GetAllAsync().ConfigureAwait(false);
        var categoriesDto = categories.ToCategoryDtos();
            
        return TypedResults.Ok(categoriesDto);
    }

    private static async Task<IResult> GetCategoryById(ICategoryRepository categoryRepository, int id)
    {
        var category = await categoryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (category == null)
        {
            return TypedResults.NotFound();
        }

        var categoryDto = category.ToCategoryDto();
        return TypedResults.Ok(categoryDto);
    }

    private static async Task<IResult> DeleteCategory(ICategoryRepository categoryRepository, int id)
    {
        var category = await categoryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (category == null)
        {
            return TypedResults.NotFound("Category not found");
        }
        
        await categoryRepository.RemoveAsync(category).ConfigureAwait(false);
        await categoryRepository.SaveChangesAsync().ConfigureAwait(false);
        
        return TypedResults.NoContent();
    }

    private static async Task<IResult> CreateCategory(ICategoryRepository categoryRepository, CreateCategoryDto createCategoryDto)
    {
        if (await categoryRepository.GetByNameAsync(createCategoryDto.Name).ConfigureAwait(false) != null)
        {
            return TypedResults.BadRequest("Coupon Name already Exists");
        }

        var category = createCategoryDto.ToCategory();

        await categoryRepository.CreateAsync(category).ConfigureAwait(false);
        await categoryRepository.SaveChangesAsync().ConfigureAwait(false);

        var categoryDto = category.ToCategoryDto();

        return TypedResults.Created($"/api/categories/{categoryDto.Id}", categoryDto);
    }
}