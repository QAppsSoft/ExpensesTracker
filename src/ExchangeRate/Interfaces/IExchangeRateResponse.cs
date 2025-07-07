namespace ExchangeRate.Interfaces;

public interface IExchangeRateResponse
{
    /// <summary>
    ///     Parses and returns the last update time in UTC as a <see cref="DateTime" /> object.
    /// </summary>
    DateTime TimeLastUpdate { get; }

    /// <summary>
    ///     Parses and returns the base currency code as a <see cref="string" /> object.
    /// </summary>
    string BaseCode { get; init; }

    /// <summary>
    ///     Parses and returns the exchange rates for each currency as a dictionary where keys are currency codes and values
    ///     are exchange rates.
    /// </summary>
    IDictionary<string, double> Rates { get; init; }
}