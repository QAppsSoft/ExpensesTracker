using Api.Data;
using Api.Endpoints.CategoryEndpoint.Repository;
using Api.Models;
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
}