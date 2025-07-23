using System.Text.Json.Serialization;
using ExchangeRate.Interfaces;

namespace ExchangeRate.Providers.BrouApi.Models;

public class BrouResponse(BrouRates brouRates, DateTime timeLastUpdate, string baseCode) : IExchangeRateResponse
{
    public DateTime TimeLastUpdate { get; } = timeLastUpdate;
    public string BaseCode { get; init; } = baseCode;

    [JsonIgnore]
    public IDictionary<string, decimal> Rates => brouRates.GetAllRates()
        .Select(x => new KeyValuePair<string, decimal>(x.CurrencyName, x.AskValue))
        .ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
}