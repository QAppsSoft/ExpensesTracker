using System.Globalization;
using System.Text.Json.Serialization;

namespace ExchangeRate.Providers.BrouApi.Models;

public record BrouCurrencyRate
{
    public string CurrencyName { get; set; }  // Added to identify the currency
    
    [JsonPropertyName("bid")]
    public string Bid { get; set; }
    
    [JsonPropertyName("ask")]
    public string Ask { get; set; }
    
    [JsonPropertyName("spread_bid")]
    public string SpreadBid { get; set; }
    
    [JsonPropertyName("spread_ask")]
    public string SpreadAsk { get; set; }

    // Helper properties to convert string values to decimal
    public decimal BidValue => ConvertToDecimal(Bid);
    public decimal AskValue => ConvertToDecimal(Ask);
    public decimal SpreadBidValue => ConvertToDecimal(SpreadBid);
    public decimal SpreadAskValue => ConvertToDecimal(SpreadAsk);

    private static decimal ConvertToDecimal(string value)
    {
        if (string.IsNullOrEmpty(value) || string.Equals(value, "-", StringComparison.Ordinal))
        {
            return 0;
        }

        // Handle numbers with comma as decimal separator
        value = value.Replace(".", "", StringComparison.Ordinal).Replace(',', '.');
        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }
        
        return 0;
    }
}