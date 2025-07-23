using System.Globalization;
using ExchangeRate.Interfaces;

namespace ExchangeRate.Providers.ExchangeRateApi.Models;

/// <summary>
///     Rates By Exchange Rate API (<a href="https://www.exchangerate-api.com">https://www.exchangerate-api.com</a>)
/// </summary>
public record ExchangeRateResponse(
    string Result,
    string Provider,
    string Documentation,
    string TermsOfUse,
    int TimeLastUpdateUnix,
    string TimeLastUpdateUtc,
    int TimeNextUpdateUnix,
    string TimeNextUpdateUtc,
    int TimeEolUnix,
    string BaseCode,
    IDictionary<string, decimal> Rates) : IExchangeRateResponse
{
    /// <summary>
    ///     Parses and returns the last update time in UTC as a <see cref="DateTime"/> object.
    /// </summary>
    public DateTime TimeLastUpdate => ParseUtcDateTime(TimeLastUpdateUtc);
    
    /// <summary>
    ///     Parses and returns the next update time in UTC as a <see cref="DateTime"/> object.
    /// </summary>
    public DateTime TimeNextUpdate => ParseUtcDateTime(TimeNextUpdateUtc);
    
    // Helper method to parse the UTC datetime strings
    private static DateTime ParseUtcDateTime(string utcString)
    {
        // Example format: "Wed, 27 Mar 2024 00:00:01 +0000"
        if (DateTime.TryParseExact(
                utcString,
                "ddd, dd MMM yyyy HH:mm:ss zzz",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var result))
        {
            return result;
        }

        // Fallback to regular parsing if format changes
        if (DateTime.TryParse(utcString, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out result))
        {
            return result;
        }

        throw new FormatException($"Could not parse datetime string: {utcString}");
    }
}