using ExchangeRate.Interfaces;

namespace ExchangeRate.Providers.OpenExchangeRatesApi.Models;

/// <summary>
///     Represents the response from the Open Exchange Rates API.
/// </summary>
/// <param name="Disclaimer">The disclaimer text.</param>
/// <param name="License">The license information.</param>
/// <param name="Timestamp">The timestamp of the response.</param>
/// <param name="Base">The base currency.</param>
/// <param name="Rates">The rates dictionary with currency codes as keys and exchange rates as values.</param>
public record OpenExchangeRatesResponse(
    string Disclaimer,
    string License,
    int Timestamp,
    string Base,
    IDictionary<string, double> Rates) : IExchangeRateResponse
{
    /// <summary>The timestamp of the response.</summary>
    public int Timestamp { get; init; } = Timestamp;

    /// <summary>The base currency.</summary>
    public string Base { get; init; } = Base;

    /// <inheritdoc cref="IExchangeRateResponse.TimeLastUpdate"/> 
    public DateTime TimeLastUpdate { get; } = DateTimeOffset.FromUnixTimeSeconds(Timestamp).UtcDateTime;

    /// <inheritdoc cref="IExchangeRateResponse.BaseCode"/>
    public string BaseCode { get; init; } = Base;
}