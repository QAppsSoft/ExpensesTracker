namespace ExchangeRate.Exceptions;

/// <summary>
///     Base exception for all exchange rate related exceptions.
/// </summary>
public class ExchangeException : Exception
{
    public ExchangeException()
    {
    }

    public ExchangeException(string? message) : base(message)
    {
    }

    public ExchangeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}