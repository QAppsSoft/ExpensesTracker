using System.Text.Json;
using ExchangeRate;
using TestsCommons;

namespace ExchangeRateFixtures;

[TestFixture]
[TestOf(typeof(CacheStorage))]
public class CacheStorageTests
{
    private TemporalStorage _temporalStorage;
    private string _cachePath;
    private CacheStorage _cacheStorage;

    [SetUp]
    public void Setup()
    {
        _temporalStorage = new TemporalStorage();
        _cachePath = _temporalStorage.GetTemporalFileName("cache-file", ".json");
        _cacheStorage = new CacheStorage(_cachePath);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _temporalStorage.Dispose();
    }
    
    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenCachePathIsNullOrWhiteSpace()
    {
        // Arrange & Act
        Action actNull = () => new CacheStorage(null);
        Action actEmpty = () => new CacheStorage("");
        Action actWhitespace = () => new CacheStorage("   ");

        // Assert
        actNull.Should().Throw<ArgumentNullException>();
        actEmpty.Should().Throw<ArgumentNullException>();
        actWhitespace.Should().Throw<ArgumentNullException>();
    }
    
    [Test]
    public void Constructor_ShouldNotThrow_WhenCachePathIsOk()
    {
        // Arrange && Act
        Action actNull = () => _ = new CacheStorage(_cachePath);

        // Assert
        actNull.Should().NotThrow();
    }
    
    [Test]
    public void Save_ShouldCreateDirectoryAndFile_WhenTheyDontExist()
    {
        // Arrange
        var testData = new List<ConversionData>
        {
            new("USD-EUR", 0.86f, DateTime.Now),
            new("EUR-GBP", 0.90f, DateTime.Now)
        };

        // Act
        _cacheStorage.Save(testData);

        // Assert
        Directory.Exists(Path.GetDirectoryName(_cachePath)).Should().BeTrue();
        File.Exists(_cachePath).Should().BeTrue();

        var fileContent = File.ReadAllText(_cachePath);
        var deserializedData = JsonSerializer.Deserialize<List<ConversionData>>(fileContent);
        deserializedData.Should().BeEquivalentTo(testData);
    }

    [Test]
    public void Save_ShouldOverwriteExistingFile_WhenItExists()
    {
        // Arrange
        var initialData = new List<ConversionData> { new("USD-EUR", 0.86f, DateTime.Now) };
        var updatedData = new List<ConversionData> { new("EUR-GBP", 0.90f, DateTime.Now) };

        // Act
        _cacheStorage.Save(initialData);
        _cacheStorage.Save(updatedData);

        // Assert
        var fileContent = File.ReadAllText(_cachePath);
        var deserializedData = JsonSerializer.Deserialize<List<ConversionData>>(fileContent);
        deserializedData.Should().BeEquivalentTo(updatedData);
    }
    
    [Test]
    public void Load_ShouldReturnEmptyCollection_WhenFileDoesNotExist()
    {
        // Arrange && Act
        var result = _cacheStorage.Load();

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Test]
    public void Load_ShouldReturnSavedData_WhenFileExists()
    {
        // Arrange
        var testData = new List<ConversionData>
        {
            new("USD-EUR", 0.86f, DateTime.Now),
            new("EUR-GBP", 0.90f, DateTime.Now)
        };

        _cacheStorage.Save(testData);

        // Act
        var result = _cacheStorage.Load();

        // Assert
        result.Should().BeEquivalentTo(testData);
    }
    
    [Test]
    public void Clear_ShouldDoNothing_WhenFileDoesNotExist()
    {
        // Act
        _cacheStorage.Clear();

        // Assert
        File.Exists(_cachePath).Should().BeFalse();
    }

    [Test]
    public void Clear_ShouldDeleteFile_WhenFileExists()
    {
        // Arrange
        var testData = new List<ConversionData>
        {
            new("USD-EUR", 0.86f, DateTime.Now)
        };

        _cacheStorage.Save(testData);

        // Act
        _cacheStorage.Clear();

        // Assert
        File.Exists(_cachePath).Should().BeFalse();
    }
    
    [Test]
    public void IntegrationTest_SaveLoadClear_WorkTogetherCorrectly()
    {
        // Initial state
        _cacheStorage.Load().Should().BeEmpty();

        // Save and verify
        var testData = new List<ConversionData>
        {
            new("USD-EUR", 0.8f, DateTime.Now)
        };
        _cacheStorage.Save(testData);
        _cacheStorage.Load().Should().BeEquivalentTo(testData);

        // Clear and verify
        _cacheStorage.Clear();
        _cacheStorage.Load().Should().BeEmpty();

        // Save again and verify
        var newTestData = new List<ConversionData>
        {
            new("EUR-GBP", 0.90f, DateTime.Now)
        };
        _cacheStorage.Save(newTestData);
        _cacheStorage.Load().Should().BeEquivalentTo(newTestData);
    }
}