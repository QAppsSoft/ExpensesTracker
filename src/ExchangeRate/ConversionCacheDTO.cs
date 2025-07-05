namespace ExchangeRate;

public record ConversionCacheDTO(DateTime LastUpdated, HashSet<ConversionData> ConversionData)
{
    public static ConversionCacheDTO Default => new(DateTime.Now, []);
}