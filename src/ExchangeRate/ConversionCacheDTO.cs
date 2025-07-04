namespace ExchangeRate;

public record ConversionCacheDTO(DateTime LastUpdated, IList<ConversionData> ConversionData)
{
    public static ConversionCacheDTO Default => new(DateTime.Now, []);
}