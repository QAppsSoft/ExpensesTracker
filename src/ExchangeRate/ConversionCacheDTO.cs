namespace ExchangeRate;

public record ConversionCacheDTO(DateTime LastUpdated, IList<ConversionData> ConversionData)
{
    public static readonly ConversionCacheDTO Default = new(DateTime.MinValue, []);
}