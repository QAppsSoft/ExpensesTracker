using System.Text.Json;
using ExchangeRate.Providers.BrouApi.Models;

namespace ExchangeRateFixtures.Providers.BrouApi.Models;

[TestFixture]
public class BrouCurrencyRateTests
{
    [Test]
    public void JsonSerializerDeserialize_BrouRates_ShouldDeserializeCorrectly()
    {
        // Arrange
        const string apiReturn = """
                                 {
                                 "dolar":{"bid":"38,90000","ask":"41,10000","spread_bid":"1,00000","spread_ask":"1,00000"},
                                 "dolar_ebrou":{"bid":"39,40000","ask":"40,60000","spread_bid":"1,00000","spread_ask":"1,00000"},
                                 "euro":{"bid":"44,56000","ask":"49,62000","spread_bid":"1,14540","spread_ask":"1,20720"},
                                 "peso_argentino":{"bid":"0,02000","ask":"0,20000","spread_bid":"2.055,00000","spread_ask":"194,50000"},
                                 "real":{"bid":"6,55000","ask":"8,25000","spread_bid":"6,27480","spread_ask":"4,71520"},
                                 "libra_esterlina":{"bid":"51,51000","ask":"57,88000","spread_bid":"1,32420","spread_ask":"1,40820"},
                                 "franco_suizo":{"bid":"48,12000","ask":"52,71000","spread_bid":"0,80840","spread_ask":"0,77970"},
                                 "guarani":{"bid":"0,00505","ask":"0,00565","spread_bid":"7.708,14000","spread_ask":"7.277,45000"},
                                 "unidad_indexada":{"bid":"-","ask":"6,36470","spread_bid":"-","spread_ask":"-"},
                                 "onza_troy_de_oro":{"bid":"132.491,45500","ask":"141.329,33700","spread_bid":"3.405,95000","spread_ask":"3.438,67000"}
                                 }
                                 """;
        
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        // Act
        var deserializedResponse = JsonSerializer.Deserialize<BrouRates>(apiReturn, options);
        
        // Assert
        deserializedResponse.Should().NotBeNull();
        deserializedResponse.GetAllRates().Should().HaveCount(10);
    }
    
    [Test]
    public void Convert_ToDecimal_ShouldHandleValidValues()
    {
        // Arrange
        var rate = new BrouCurrencyRate
        {
            Bid = "1.234,56",
            Ask = "7.890,12",
            SpreadBid = "0,12",
            SpreadAsk = "0,34"
        };

        // Act & Assert
        rate.BidValue.Should().Be(1234.56m);
        rate.AskValue.Should().Be(7890.12m);
        rate.SpreadBidValue.Should().Be(0.12m);
        rate.SpreadAskValue.Should().Be(0.34m);
    }

    [Test]
    public void Convert_ToDecimal_ShouldHandleEmptyValues()
    {
        // Arrange
        var rate = new BrouCurrencyRate
        {
            Bid = "",
            Ask = "-",
            SpreadBid = null,
            SpreadAsk = ""
        };

        // Act & Assert
        rate.BidValue.Should().Be(0m);
        rate.AskValue.Should().Be(0m);
        rate.SpreadBidValue.Should().Be(0m);
        rate.SpreadAskValue.Should().Be(0m);
    }
}