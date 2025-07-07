using ExchangeRate.Providers;
using ExchangeRate.Providers.Interfaces;
using ExchangeRate.Providers.Models;

namespace ExchangeRateFixtures.Providers
{
    [TestFixture]
    public class ExchangeProviderTests
    {
        private Mock<IProvider> _mockProvider1;
        private Mock<IProvider> _mockProvider2;
        private List<IProvider> _providers;
        private ExchangeProvider _exchangeProvider;
        private IProviderSelector _providerSelector;

        [SetUp]
        public void SetUp()
        {
            _mockProvider1 = new Mock<IProvider>();
            _mockProvider2 = new Mock<IProvider>();
            _providers = [_mockProvider1.Object, _mockProvider2.Object];
            _providerSelector = new FirstProviderSelector();
            _exchangeProvider = new ExchangeProvider(_providers, _providerSelector);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenProvidersIsNull()
        {
            // Act
            Action act = () => _ = new ExchangeProvider(null, _providerSelector);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*providers*");
        }

        [Test]
        public async Task GetRatesAsync_ShouldReturnRatesFromFirstProvider()
        {
            // Arrange
            var expectedRates = new List<CurrencyPairRate>
            {
                new("USD-EUR", 0.85, DateTime.UtcNow),
                new("USD-GBP", 0.75, DateTime.UtcNow)
            };
            _mockProvider1.Setup(p => p.GetRatesAsync("USD")).ReturnsAsync(expectedRates);

            // Act
            var result = await _exchangeProvider.GetRatesAsync("USD");

            // Assert
            result.Should().BeEquivalentTo(expectedRates);
            _mockProvider1.Verify(p => p.GetRatesAsync("USD"), Times.Once);
            _mockProvider2.Verify(p => p.GetRatesAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task GetRatesByProviderAsync_ShouldReturnRatesFromSpecifiedProvider()
        {
            // Arrange
            var expectedRates = new List<CurrencyPairRate>
            {
                new("USD-EUR", 0.85, DateTime.UtcNow),
                new("USD-GBP", 0.75, DateTime.UtcNow)
            };
            _mockProvider2.Setup(p => p.GetRatesAsync("USD")).ReturnsAsync(expectedRates);

            // Act
            var result = await _exchangeProvider.GetRatesByProviderAsync(_mockProvider2.Object, "USD");

            // Assert
            result.Should().BeEquivalentTo(expectedRates);
            _mockProvider2.Verify(p => p.GetRatesAsync("USD"), Times.Once);
            _mockProvider1.Verify(p => p.GetRatesAsync(It.IsAny<string>()), Times.Never);
        }
    }
}