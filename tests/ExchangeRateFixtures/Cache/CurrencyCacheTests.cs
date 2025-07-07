using System.Text.Json;
using ExchangeRate.Cache;
using ExchangeRate.Providers.Models;
using TestsCommons;

namespace ExchangeRateFixtures.Cache
{
    [TestFixture]
    public class CurrencyCacheTests
    {
        private string _testCachePath;
        private CurrencyCache _currencyCache;
        private CacheStorage _cacheStorage;
        private TemporalStorage _temporalStorage;

        [SetUp]
        public void Setup()
        {
            _temporalStorage = new TemporalStorage();
            _testCachePath = _temporalStorage.GetTemporalFileName("test_cache", ".json");
            _cacheStorage = new CacheStorage(_testCachePath);
            _currencyCache = new CurrencyCache(_cacheStorage);
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
            Action act = () => _ = new CurrencyCache(new CacheStorage(string.Empty));

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Constructor_WhenCacheFileExists_LoadsExistingData()
        {
            // Arrange
            var testData = new CurrencyPairRate[] { new("USD", "EUR", 0.85f, DateTime.Now) };

            var serializedData = JsonSerializer.Serialize(testData);
            File.WriteAllText(_testCachePath, serializedData);

            // Act
            var cache = new CurrencyCache(_cacheStorage);
            
            // Assert
            var cachedItem = cache.GetCachedConversionData("USD", "EUR");
            cachedItem.Should().NotBeNull();
            cachedItem.FromCurrency.Should().Be("USD");
            cachedItem.ToCurrency.Should().Be("EUR");
            cachedItem.Rate.Should().Be(0.85f);
        }

        [Test]
        public void SaveToCacheData_WithSingleNewItem_AddsItemToCache()
        {
            // Arrange
            var testData = new CurrencyPairRate("USD", "EUR", 0.85f, DateTime.Now);

            // Act
            _currencyCache.SaveToCacheData(testData);

            // Assert
            var cachedItem = _currencyCache.GetCachedConversionData("USD", "EUR");
            cachedItem.Should().NotBeNull();
            cachedItem.FromCurrency.Should().Be("USD");
            cachedItem.ToCurrency.Should().Be("EUR");
            cachedItem.Rate.Should().Be(0.85f);

            // Verify only one item exists in cache
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<List<CurrencyPairRate>>(fileContent);
            deserialized.Should().ContainSingle(d => d.FromCurrency == "USD" && d.ToCurrency == "EUR");
        }

        [Test]
        public void SaveToCacheData_WithExistingItem_UpdatesItemInCache()
        {
            // Arrange
            var initialData = new CurrencyPairRate("USD", "EUR", 0.85f, DateTime.Now);
            _currencyCache.SaveToCacheData(initialData);

            var updatedData = new CurrencyPairRate("USD", "EUR", 0.86f, DateTime.Now);

            // Act
            _currencyCache.SaveToCacheData(updatedData);

            // Assert
            var cachedItem = _currencyCache.GetCachedConversionData("USD", "EUR");
            cachedItem.Should().NotBeNull();
            cachedItem.FromCurrency.Should().Be("USD");
            cachedItem.ToCurrency.Should().Be("EUR");
            cachedItem.Rate.Should().Be(0.86f);

            // Verify file was updated
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<List<CurrencyPairRate>>(fileContent);
            deserialized.Should().ContainSingle(d => d.FromCurrency == "USD" && d.ToCurrency == "EUR");
        }

        [Test]
        public void SaveToCacheData_WithMultipleItems_AddsOrUpdatesAllItems()
        {
            // Arrange
            var initialData = new CurrencyPairRate("USD", "EUR", 0.85f, DateTime.Now);
            _currencyCache.SaveToCacheData(initialData);

            var testData = new List<CurrencyPairRate>
            {
                new("USD", "EUR", 0.86f, DateTime.Now),
                new("EUR", "GBP", 0.90f, DateTime.Now)
            };

            // Act
            _currencyCache.SaveToCacheData(testData);

            // Assert
            _currencyCache.GetCachedConversionData("USD", "EUR").Rate.Should().Be(0.86f);
            _currencyCache.GetCachedConversionData("EUR", "GBP").Should().NotBeNull();

            // Verify file contains both items
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<List<CurrencyPairRate>>(fileContent);
            deserialized.Should().HaveCount(2);
        }

        [Test]
        public void GetCachedConversionData_WithNonExistingKey_ReturnsNull()
        {
            // Act
            var result = _currencyCache.GetCachedConversionData("NON", "EXISTENT");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void ResetCache_ClearsAllDataAndCreatesDefaultCache()
        {
            // Arrange
            var testData = new List<CurrencyPairRate>
            {
                new("USD", "EUR", 0.85f, DateTime.Now),
                new("EUR", "GBP", 0.90f, DateTime.Now)
            };
            _currencyCache.SaveToCacheData(testData);

            // Act
            _currencyCache.ResetCache();

            // Assert
            _currencyCache.GetCachedConversionData("USD", "EUR").Should().BeNull();
            _currencyCache.GetCachedConversionData("EUR", "GBP").Should().BeNull();

            // Verify file was reset to default
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<List<CurrencyPairRate>>(fileContent);
            deserialized.Should().BeEquivalentTo(new List<CurrencyPairRate>());
        }

        [Test]
        public void ResetCache_WithOnlyExpiredSetTrue_ClearsOnlyExpiredCacheData()
        {
            var convData1 = new CurrencyPairRate("USD", "EUR", 0.85f,
                DateTime.Now - CurrencyCache.CacheDuration.Add(TimeSpan.FromHours(1)));
            var convData2 = new CurrencyPairRate("EUR", "GBP", 0.90f,
                DateTime.Now - CurrencyCache.CacheDuration.Add(TimeSpan.FromHours(-1)));

            // Arrange
            var testData = new List<CurrencyPairRate> { convData1, convData2 };
            _currencyCache.SaveToCacheData(testData);

            // Act
            _currencyCache.ResetCache(true);

            // Assert
            _currencyCache.GetCachedConversionData("USD", "EUR").Should().BeNull();
            _currencyCache.GetCachedConversionData("EUR", "GBP").Should().NotBeNull();

            // Verify file was reset to default
            var fileContent = File.ReadAllText(_testCachePath);
            var deserialized = JsonSerializer.Deserialize<List<CurrencyPairRate>>(fileContent);
            deserialized.Should().BeEquivalentTo(new List<CurrencyPairRate> { convData2 });
        }
    }
}