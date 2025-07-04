using ExchangeRate;

namespace ExchangeRateFixtures;

[TestFixture]
public class CurrencyInfoTests
{
    [Test]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var code = "USD";
        var name = "US Dollar";
        var symbol = "$";

        // Act
        var currency = new CurrencyInfo(code, name, symbol);

        // Assert
        currency.Code.Should().Be(code);
        currency.Name.Should().Be(name);
        currency.Symbol.Should().Be(symbol);
    }

    [Test]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var currency = new CurrencyInfo("EUR", "Euro", "€");

        // Act & Assert
        currency.ToString().Should().Be("Euro - €");
    }

    [Test]
    public void Equals_WithSameCode_ShouldReturnTrue()
    {
        // Arrange
        var currency1 = new CurrencyInfo("JPY", "Japanese Yen", "¥");
        var currency2 = new CurrencyInfo("JPY", "Japanese Yen", "¥");

        // Act & Assert
        currency1.Equals(currency2).Should().BeTrue();
    }

    [Test]
    public void Equals_WithDifferentCode_ShouldReturnFalse()
    {
        // Arrange
        var currency1 = new CurrencyInfo("GBP", "British Pound", "£");
        var currency2 = new CurrencyInfo("USD", "US Dollar", "$");

        // Act & Assert
        currency1.Equals(currency2).Should().BeFalse();
    }

    [Test]
    public void Equals_WithNonCurrencyInfoObject_ShouldReturnFalse()
    {
        // Arrange
        var currency = new CurrencyInfo("CAD", "Canadian Dollar", "CA$");
        var otherObject = new object();

        // Act & Assert
        currency.Equals(otherObject).Should().BeFalse();
    }

    [Test]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var currency = new CurrencyInfo("AUD", "Australian Dollar", "A$");

        // Act & Assert
        currency.Equals(null).Should().BeFalse();
    }

    [Test]
    public void GetHashCode_ForSameCode_ShouldReturnSameValue()
    {
        // Arrange
        var currency1 = new CurrencyInfo("CHF", "Swiss Franc", "CHF");
        var currency2 = new CurrencyInfo("CHF", "Swiss Franc", "Fr.");

        // Act & Assert
        currency1.GetHashCode().Should().Be(currency2.GetHashCode());
    }

    [Test]
    public void GetHashCode_ForDifferentCode_ShouldReturnDifferentValue()
    {
        // Arrange
        var currency1 = new CurrencyInfo("CNY", "Chinese Yuan", "¥");
        var currency2 = new CurrencyInfo("HKD", "Hong Kong Dollar", "HK$");

        // Act & Assert
        currency1.GetHashCode().Should().NotBe(currency2.GetHashCode());
    }

    [Test]
    public void GenerateCurrencyList_ShouldReturnNonEmptyCollection()
    {
        // Act
        var currencies = CurrencyInfo.GenerateCurrencyList();

        // Assert
        currencies.Should().NotBeNull();
        currencies.Should().NotBeEmpty();
    }

    [Test]
    public void GenerateCurrencyList_ShouldContainCommonCurrencies()
    {
        // Act
        var currencies = CurrencyInfo.GenerateCurrencyList().ToList();

        // Assert
        currencies.Should().Contain(c => c.Code == "USD");
        currencies.Should().Contain(c => c.Code == "EUR");
        currencies.Should().Contain(c => c.Code == "JPY");
        currencies.Should().Contain(c => c.Code == "GBP");
    }

    [Test]
    public void GenerateCurrencyList_ShouldHaveUniqueCurrencyCodes()
    {
        // Act
        var currencies = CurrencyInfo.GenerateCurrencyList().ToList();
        var currencyCodes = currencies.Select(c => c.Code).ToList();

        // Assert
        currencyCodes.Should().OnlyHaveUniqueItems();
    }

    [Test]
    public void GenerateCurrencyList_ItemsShouldHaveAllPropertiesSet()
    {
        // Act
        var currencies = CurrencyInfo.GenerateCurrencyList();

        // Assert
        foreach (var currency in currencies)
        {
            currency.Code.Should().NotBeNullOrWhiteSpace();
            currency.Name.Should().NotBeNullOrWhiteSpace();
            currency.Symbol.Should().NotBeNullOrWhiteSpace();
        }
    }
}