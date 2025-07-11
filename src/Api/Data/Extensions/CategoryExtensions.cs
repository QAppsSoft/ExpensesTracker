using Api.Models.Dto.Categories;

namespace Api.Data.Extensions;

public static class CategoryExtensions
{
    public static CategoryDto ToCategoryDto(this Category category) =>
        new(category.Id, category.Name, category.Description, category.Color);

    public static Category ToCategory(this CategoryDto categoryDto) =>
        new()
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            Color = categoryDto.Color,
        };

    public static Category ToCategory(this CategoryCreateDto createCategoryDto) =>
        new()
        {
            Name = createCategoryDto.Name,
            Description = createCategoryDto.Description,
            Color = createCategoryDto.Color,
        };

    public static IEnumerable<CategoryDto> ToCategoryDtos(this IEnumerable<Category> categories) =>
        categories.Select(c => c.ToCategoryDto());
}