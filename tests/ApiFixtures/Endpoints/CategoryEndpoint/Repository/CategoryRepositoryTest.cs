using Api.Data;
using Api.Data.Context;
using Api.Endpoints.CategoryEndpoint.Repository;
using Microsoft.EntityFrameworkCore;
using TestsCommons.Db;

namespace ApiFixtures.Endpoints.CategoryEndpoint.Repository;

[TestFixture]
[TestOf(typeof(CategoryRepository))]
public class CategoryRepositoryTests
{
    private DbContextFactoryFixture _dbContextFactory;
    private ExpensesTrackerDbContext _context;
    private CategoryRepository _categoryRepository;

    [SetUp]
    public void Setup()
    {
        _dbContextFactory = new DbContextFactoryFixture();
        _context = _dbContextFactory.CreateDbContext();
        
        _context.Categories.AddRange(
            new Category { Name = "Food", Color = "#FF5733" },
            new Category { Name = "Travel", Color = "#33FF57" });
        _context.SaveChanges();
        
        _categoryRepository = new CategoryRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContextFactory.Dispose();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllCategories()
    {
        // Act
        var categories = await _categoryRepository.GetAllAsync();

        // Assert
        categories.Should().NotBeNull();
        categories.Should().HaveCount(2);
        categories.Should().Contain(c => c.Name == "Food" && c.Color == "#FF5733");
        categories.Should().Contain(c => c.Name == "Travel" && c.Color == "#33FF57");
    }


    [Test]
    public async Task GetByNameAsync_ExistingName_ReturnsCategory()
    {
        // Arrange
        const string name = "Food";

        // Act
        var category = await _categoryRepository.GetByNameAsync(name);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(name);
    }

    [Test]
    public async Task GetByNameAsync_NonExistingName_ReturnsNull()
    {
        // Arrange
        const string name = "NonExisting";

        // Act
        var category = await _categoryRepository.GetByNameAsync(name);

        // Assert
        category.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsCategory()
    {
        // Arrange
        const int id = 1;

        // Act
        var category = await _categoryRepository.GetByIdAsync(id);

        // Assert
        category.Should().NotBeNull();
        category.Id.Should().Be(id);
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        const int id = 99;

        // Act
        var category = await _categoryRepository.GetByIdAsync(id);

        // Assert
        category.Should().BeNull();
    }

    [Test]
    public async Task CreateAsync_ValidCategory_AddsCategoryToDatabase()
    {
        // Arrange
        var newCategory = new Category { Name = "Utilities", Description = "Utility bills", Color = "#0000FF" };

        // Act
        await _categoryRepository.CreateAsync(newCategory);
        await _context.SaveChangesAsync();

        // Assert
        var categoryInDb = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Utilities");
        categoryInDb.Should().NotBeNull();
        categoryInDb.Name.Should().Be("Utilities");
        categoryInDb.Description.Should().Be("Utility bills");
        categoryInDb.Color.Should().Be("#0000FF");
    }

    [Test]
    public async Task RemoveAsync_ExistingCategory_RemovesCategoryFromDatabase()
    {
        // Arrange
        var categoryToRemove = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Food");

        // Act
        await _categoryRepository.RemoveAsync(categoryToRemove);
        await _context.SaveChangesAsync();

        // Assert
        var categoryInDb = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Food");
        categoryInDb.Should().BeNull();
    }

    [Test]
    public async Task SaveChangesAsync_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var newCategory = new Category { Name = "TestCategory", Color = "#FFFFFF" };
        _context.Categories.Add(newCategory);

        // Act
        await _categoryRepository.SaveChangesAsync();

        // Assert
        _context.Categories.Should().Contain(newCategory);
    }

    [Test]
    public async Task UpdateAsync_ExistingCategory_UpdatesCategoryInDatabase()
    {
        // Arrange
        var categoryToUpdate = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Food");
        categoryToUpdate.Name = "UpdatedFood";
        categoryToUpdate.Description = "Updated description";
        categoryToUpdate.Color = "#00FF00";

        // Act
        await _categoryRepository.UpdateAsync(categoryToUpdate);
        await _context.SaveChangesAsync();

        // Assert
        var updatedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "UpdatedFood");
        updatedCategory.Should().NotBeNull();
        updatedCategory.Description.Should().Be("Updated description");
    }

    [Test]
    public async Task UpdateAsync_NonExistingCategory_ThrowsException()
    {
        // Arrange
        var nonExistingCategory = new Category { Id = 999, Name = "NonExisting", Color = "#FFFFFF" };

        // Act
        Func<Task> act = async () => await _categoryRepository.UpdateAsync(nonExistingCategory);
        
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Category with ID {nonExistingCategory.Id} not found.");
    }

    [Test]
    public async Task ExistsByIdAsync_ExistingCategory_ShouldReturnTrue()
    {
        // Arrange
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Food");
        
        // Act
        var exists = await _categoryRepository.ExistsByIdAsync(category.Id);
        
        // Assert
        exists.Should().BeTrue();
    }
    
    [Test]
    public async Task ExistsByIdAsync_NonExistingCategory_ShouldReturnFalse()
    {
        // Act
        var exists = await _categoryRepository.ExistsByIdAsync(999);
        
        // Assert
        exists.Should().BeFalse();
    }
}