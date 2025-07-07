using ExchangeRate;
using ExchangeRate.Providers.Interfaces;
using ExchangeRate.Providers.Models;

namespace ExchangeRateFixtures;

[TestFixture]
[TestOf(typeof(CurrencyConversionProvider))]
public class CurrencyConversionProviderTest
{
    private Mock<IExchangeProvider> _mockExchangeProvider;
    private CurrencyConversionProvider _currencyConversionProvider;

    [SetUp]
    public void Setup()
    {
        _mockExchangeProvider = new Mock<IExchangeProvider>();
        _currencyConversionProvider = new CurrencyConversionProvider(_mockExchangeProvider.Object);
    }

    [Test]
    public async Task ConvertAsync_WithValidRates_ReturnsConvertedAmount()
    {
        // Arrange
        var rates = new List<CurrencyPairRate>
        {
            new("USD", "EUR", 0.85, DateTime.UtcNow)
        };
        _mockExchangeProvider.Setup(p => p.GetRatesAsync("USD")).ReturnsAsync(rates);

        // Act
        var result = await _currencyConversionProvider.ConvertAsync(100, "USD", "EUR");

        // Assert
        result.Should().Be(85);
        _mockExchangeProvider.Verify(p => p.GetRatesAsync("USD"), Times.Once);
    }

    [Test]
    public async Task ConvertAsync_WithNoMatchingRate_ThrowsInvalidOperationException()
    {
        // Arrange
        var rates = new List<CurrencyPairRate>
        {
            new("USD", "GBP", 0.75, DateTime.UtcNow)
        };
        _mockExchangeProvider.Setup(p => p.GetRatesAsync("USD")).ReturnsAsync(rates);

        // Act
        Func<Task> act = async () => await _currencyConversionProvider.ConvertAsync(100, "USD", "EUR");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Rate for USD-EUR not found.");
        _mockExchangeProvider.Verify(p => p.GetRatesAsync("USD"), Times.Once);
    }

    [Test]
    public async Task ConvertAsync_WithEmptyRates_ThrowsInvalidOperationException()
    {
        // Arrange
        var rates = new List<CurrencyPairRate>();
        _mockExchangeProvider.Setup(p => p.GetRatesAsync("USD")).ReturnsAsync(rates);

        // Act
        Func<Task> act = async () => await _currencyConversionProvider.ConvertAsync(100, "USD", "EUR");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Rate for USD-EUR not found.");
        _mockExchangeProvider.Verify(p => p.GetRatesAsync("USD"), Times.Once);
    }
}