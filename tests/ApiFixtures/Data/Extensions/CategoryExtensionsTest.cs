using Api.Data;
using Api.Data.Extensions;
using Api.Models.Dto.Categories;

namespace ApiFixtures.Data.Extensions;

[TestFixture]
[TestOf(typeof(CategoryExtensions))]
public class CategoryExtensionsTest
{
    [Test]
    public void ToCategoryDto_ConvertsCategoryToCategoryDtoCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description",
            Color = "#FFFFFF"
        };

        // Act
        var result = category.ToCategoryDto();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
        result.Description.Should().Be(category.Description);
        result.Color.Should().Be(category.Color);
    }

    [Test]
    public void ToCategory_ConvertsCategoryDtoToCategoryCorrectly()
    {
        // Arrange
        var categoryDto = new CategoryDto(
            1,
            "Test Category",
            "Test Description",
            "#FFFFFF");

        // Act
        var result = categoryDto.ToCategory();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(categoryDto.Id);
        result.Name.Should().Be(categoryDto.Name);
        result.Description.Should().Be(categoryDto.Description);
        result.Color.Should().Be(categoryDto.Color);
    }

    [Test]
    public void ToCategory_ConvertsCreateCategoryDtoToCategoryCorrectly()
    {
        // Arrange
        var createCategoryDto = new CreateCategoryDto(
            "Test Category",
            "Test Description",
            "#FFFFFF");

        // Act
        var result = createCategoryDto.ToCategory();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(0); // default value for int
        result.Name.Should().Be(createCategoryDto.Name);
        result.Description.Should().Be(createCategoryDto.Description);
        result.Color.Should().Be(createCategoryDto.Color);
    }

    [Test]
    public void ToCategoryDtos_ConvertsCategoryCollectionToCategoryDtoCollectionCorrectly()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Cat1", Description = "Desc1", Color = "#111111" },
            new Category { Id = 2, Name = "Cat2", Description = "Desc2", Color = "#222222" },
            new Category { Id = 3, Name = "Cat3", Description = "Desc3", Color = "#333333" }
        };

        // Act
        var result = categories.ToCategoryDtos().ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        result[0].Id.Should().Be(1);
        result[0].Name.Should().Be("Cat1");
        result[0].Description.Should().Be("Desc1");
        result[0].Color.Should().Be("#111111");

        result[1].Id.Should().Be(2);
        result[1].Name.Should().Be("Cat2");
        result[1].Description.Should().Be("Desc2");
        result[1].Color.Should().Be("#222222");

        result[2].Id.Should().Be(3);
        result[2].Name.Should().Be("Cat3");
        result[2].Description.Should().Be("Desc3");
        result[2].Color.Should().Be("#333333");
    }

    [Test]
    public void ToCategoryDtos_WithEmptyCollection_ReturnsEmptyCollection()
    {
        // Arrange
        var emptyCategories = Enumerable.Empty<Category>();

        // Act
        var result = emptyCategories.ToCategoryDtos();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}