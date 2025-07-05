using System.Text.Json;
using ExchangeRate;
using TestsCommons;

namespace ExchangeRateTests
{
    [TestFixture]
    public class CurrencyCacheTests
    {
        private string _testCachePath;
        private CurrencyCache _currencyCache;
        private TemporalStorage _temporalStorage;

        [SetUp]
        public void Setup()
        {
            _temporalStorage = new TemporalStorage();
            _testCachePath = _temporalStorage.GetTemporalFileName("test_cache", ".json");
            _currencyCache = new CurrencyCache(_testCachePath);
        }

        [TearDown]
        public void Cleanup()
        {
            _temporalStorage.Dispose();
        }

        [Test]
        public void Constructor_WithNullCachePath_ThrowsArgumentNullException()
        {
            // Act
            Action act = () => _ = new CurrencyCache(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WithEmptyCachePath_ThrowsArgumentNullException()
        {
            // Act
            Action act = () => _ = new CurrencyCache(string.Empty);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCacheFileDoesNotExist_CreatesDefaultCacheFile()
        {
            // Arrange
            var cachePath = Path.Combine(_temporalStorage.GetTemporalDirectory(), 
                "other_folder",
                "other_cache_file.json");

            // Act
            _ = new CurrencyCache(cachePath);

            // Assert
            File.Exists(cachePath).Should().BeTrue();
            var fileContent = File.ReadAllText(cachePath);
            var deserialized = JsonSerializer.Deserialize<ConversionCacheDTO>(fileContent);
            deserialized.LastUpdated.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            deserialized.ConversionData.Should().BeEquivalentTo(new List<ConversionData>());
        }

        [Test]
        public void Constructor_WhenCacheFileExists_LoadsExistingData()
        {
            // Arrange
            var testData = new ConversionCacheDTO(DateTime.Now, [new ConversionData("USD-EUR", 0.85f)]);
            
            var serializedData = JsonSerializer.Serialize(testData);
            File.WriteAllText(_testCachePath, serializedData);

            // Act
            var cache = new CurrencyCache(_testCachePath);

            // Assert
            cache.GetCachedConversionData("USD-EUR").Should().NotBeNull();
            cache.GetCachedConversionData("USD-EUR").Value.Should().Be(0.85f);
        }

        [Test]
        public void SaveToCacheData_WithSingleNewItem_AddsItemToCache()
        {
            // Arrange
            var testData = new ConversionData("USD-EUR", 0.85f);

            // Act
            _currencyCache.SaveToCacheData(testData);

            // Assert
            var cachedItem = _currencyCache.GetCachedConversionData("USD-EUR");
            cachedItem.Should().NotBeNull();
            cachedItem.Key.Should().Be("USD-EUR");
            cachedItem.Value.Should().Be(0.85f);

            // Verify file was updated
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<ConversionCacheDTO>(fileContent);
            deserialized.ConversionData.Should().ContainSingle(d => d.Key == "USD-EUR");
        }

        [Test]
        public void SaveToCacheData_WithExistingItem_UpdatesItemInCache()
        {
            // Arrange
            var initialData = new ConversionData("USD-EUR", 0.85f);
            _currencyCache.SaveToCacheData(initialData);

            var updatedData = new ConversionData("USD-EUR", 0.86f);

            // Act
            _currencyCache.SaveToCacheData(updatedData);

            // Assert
            var cachedItem = _currencyCache.GetCachedConversionData("USD-EUR");
            cachedItem.Should().NotBeNull();
            cachedItem.Value.Should().Be(0.86f);

            // Verify only one item exists in cache
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<ConversionCacheDTO>(fileContent);
            deserialized.ConversionData.Should().ContainSingle(d => d.Key == "USD-EUR");
        }

        [Test]
        public void SaveToCacheData_WithMultipleItems_AddsOrUpdatesAllItems()
        {
            // Arrange
            var initialData = new ConversionData("USD-EUR", 0.85f);
            _currencyCache.SaveToCacheData(initialData);

            var testData = new List<ConversionData>
            {
                new("USD-EUR", 0.86f),
                new("EUR-GBP", 0.90f)
            };

            // Act
            _currencyCache.SaveToCacheData(testData);

            // Assert
            _currencyCache.GetCachedConversionData("USD-EUR").Value.Should().Be(0.86f);
            _currencyCache.GetCachedConversionData("EUR-GBP").Should().NotBeNull();

            // Verify file contains both items
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<ConversionCacheDTO>(fileContent);
            deserialized.ConversionData.Should().HaveCount(2);
        }

        [Test]
        public void GetCachedConversionData_WithNonExistingKey_ReturnsNull()
        {
            // Act
            var result = _currencyCache.GetCachedConversionData("NON-EXISTENT");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void ResetCache_ClearsAllDataAndCreatesDefaultCache()
        {
            // Arrange
            var testData = new List<ConversionData>
            {
                new("USD-EUR", 0.85f),
                new("EUR-GBP", 0.90f)
            };
            _currencyCache.SaveToCacheData(testData);

            // Act
            _currencyCache.ResetCache();

            // Assert
            _currencyCache.GetCachedConversionData("USD-EUR").Should().BeNull();
            _currencyCache.GetCachedConversionData("EUR-GBP").Should().BeNull();

            // Verify file was reset to default
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<ConversionCacheDTO>(fileContent);
            deserialized.LastUpdated.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            deserialized.ConversionData.Should().BeEquivalentTo(new List<ConversionData>());
        }

        [Test]
        public void Constructor_WhenParentDirectoryDoesNotExist_CreatesDirectory()
        {
            // Arrange
            var cachePath = _temporalStorage.GetTemporalFileName("other-cache", ".json");
            var newDir = Path.GetDirectoryName(cachePath);

            // Act
            _ = new CurrencyCache(cachePath);

            // Assert
            Directory.Exists(newDir).Should().BeTrue();
            File.Exists(cachePath).Should().BeTrue();
        }
    }
}