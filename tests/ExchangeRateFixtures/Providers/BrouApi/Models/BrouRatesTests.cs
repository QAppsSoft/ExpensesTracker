using ExchangeRate.Providers.BrouApi.Models;

namespace ExchangeRateFixtures.Providers.BrouApi.Models;

[TestFixture]
public class BrouRatesTests
{
    [Test]
    public void GetAllRates_ShouldReturnAllRatesWithCorrectCurrencyNames()
    {
        // Arrange
        var rates = new BrouRates
        {
            Dolar = new BrouCurrencyRate { Bid = "1.00", Ask = "1.10" },
            DolarEbrou = new BrouCurrencyRate { Bid = "1.05", Ask = "1.15" },
            Euro = new BrouCurrencyRate { Bid = "0.85", Ask = "0.95" },
            PesoArgentino = new BrouCurrencyRate { Bid = "90.00", Ask = "95.00" },
            Real = new BrouCurrencyRate { Bid = "5.00", Ask = "5.50" },
            LibraEsterlina = new BrouCurrencyRate { Bid = "0.75", Ask = "0.85" },
            FrancoSuizo = new BrouCurrencyRate { Bid = "1.00", Ask = "1.10" },
            Guarani = new BrouCurrencyRate { Bid = "6000.00", Ask = "6500.00" },
            UnidadIndexada = new BrouCurrencyRate { Bid = "1.00", Ask = "1.10" },
            OnzaTroyDeOro = new BrouCurrencyRate { Bid = "1500.00", Ask = "1600.00" }
        };

        // Act
        var allRates = rates.GetAllRates().ToList();

        // Assert
        allRates.Should().HaveCount(10);
        allRates.Should().ContainSingle(r => r.CurrencyName == "USD");
        allRates.Should().ContainSingle(r => r.CurrencyName == "USD_EBROU");
        allRates.Should().ContainSingle(r => r.CurrencyName == "EUR");
        allRates.Should().ContainSingle(r => r.CurrencyName == "ARS");
        allRates.Should().ContainSingle(r => r.CurrencyName == "BRL");
        allRates.Should().ContainSingle(r => r.CurrencyName == "GBP");
        allRates.Should().ContainSingle(r => r.CurrencyName == "CHF");
        allRates.Should().ContainSingle(r => r.CurrencyName == "PYG");
        allRates.Should().ContainSingle(r => r.CurrencyName == "UNIDAD_INDEXADA");
        allRates.Should().ContainSingle(r => r.CurrencyName == "XAU");
    }
}