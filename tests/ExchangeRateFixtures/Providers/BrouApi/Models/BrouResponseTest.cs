using System.Text.Json;
using ExchangeRate.Providers.BrouApi.Models;

namespace ExchangeRateFixtures.Providers.BrouApi.Models;

[TestFixture]
[TestOf(typeof(BrouResponse))]
public class BrouResponseTest
{
    private BrouRates _brouRates;
    private DateTime _timeLastUpdate;
    private string _baseCode;

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

    [SetUp]
    public void SetUp()
    {
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
        _brouRates = JsonSerializer.Deserialize<BrouRates>(apiReturn, options); 
        
        _timeLastUpdate = DateTime.Now;
        _baseCode = "UYU";
    }

    [Test]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var expectedRates = new Dictionary<string, decimal>
        {
            { "USD", 41.1m },
            { "EUR", 49.62m }
        };

        // Act
        var brouResponse = new BrouResponse(_brouRates, _timeLastUpdate, _baseCode);

        // Assert
        brouResponse.TimeLastUpdate.Should().Be(_timeLastUpdate);
        brouResponse.BaseCode.Should().Be(_baseCode);
        brouResponse.Rates.Count.Should().Be(10);
        brouResponse.Rates.Should().Contain(expectedRates);
    }

    [Test]
    public void GetRates_ShouldReturnCorrectRates_WhenBaseCodeIsUYU()
    {
        // Arrange
        var expectedRates = new Dictionary<string, decimal>
        {
            { "USD", 41.1m },
            { "EUR", 49.62m }
        };

        // Act
        var brouResponse = new BrouResponse(_brouRates, _timeLastUpdate, _baseCode);

        // Assert
        brouResponse.Rates.Count.Should().Be(10);
        brouResponse.Rates.Should().Contain(expectedRates);
    }

    [Test]
    public void GetRates_ShouldReturnCorrectRates_WhenBaseCodeIsNotUYU()
    {
        // Arrange
        _baseCode = "USD";
        var expectedRates = new Dictionary<string, decimal>
        {
            { "UYU", 38.9m }
        };

        // Act
        var brouResponse = new BrouResponse(_brouRates, _timeLastUpdate, _baseCode);

        // Assert
        brouResponse.Rates.Should().BeEquivalentTo(expectedRates);
    }
}