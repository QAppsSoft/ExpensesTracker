using System.Text.Json.Serialization;
using ExchangeRate.Interfaces;

namespace ExchangeRate.Providers.BrouApi.Models;

public class BrouResponse(BrouRates brouRates, DateTime timeLastUpdate, string baseCode) : IExchangeRateResponse
{
    public DateTime TimeLastUpdate { get; } = timeLastUpdate;
    public string BaseCode { get; init; } = baseCode;

    public IDictionary<string, decimal> Rates => GetRates();

    private Dictionary<string, decimal> GetRates()
    {
        var rates = brouRates.GetAllRates();

        if (string.Equals(BaseCode, "UYU", StringComparison.Ordinal))
        {
            return rates.Select(x => new KeyValuePair<string, decimal>(x.CurrencyName, x.AskValue))
                .ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
        }

        return rates.Where(x => string.Equals(x.CurrencyName, BaseCode, StringComparison.Ordinal))
            .Select(x => new KeyValuePair<string, decimal>("UYU", x.BidValue))
            .ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
    }
}