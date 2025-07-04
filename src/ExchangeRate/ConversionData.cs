namespace ExchangeRate;

public sealed record ConversionData(string Key, float Value)
{
    public bool Equals(ConversionData? obj)
    {
        return obj != null && string.Equals(Key, obj.Key, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return StringComparer.Ordinal.GetHashCode(Key);
    }
}