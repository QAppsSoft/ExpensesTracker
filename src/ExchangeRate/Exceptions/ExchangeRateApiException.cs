namespace ExchangeRate.Exceptions;

/// <summary>
///     Exception thrown when an error occurs while interacting with the Exchange Rate API.
/// </summary>
public class ExchangeRateApiException : ExchangeException
{
    public ExchangeRateApiException()
    {
    }

    public ExchangeRateApiException(string? message) : base(message)
    {
    }

    public ExchangeRateApiException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}