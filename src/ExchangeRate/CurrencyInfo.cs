using System.Globalization;

namespace ExchangeRate;

[Serializable]
public sealed record CurrencyInfo(string Code, string Name, string Symbol)
{
    public override string ToString()
    {
        return $"{Name} - {Symbol}";
    }

    public bool Equals(CurrencyInfo? obj)
    {
        return obj != null && string.Equals(Code, obj.Code, StringComparison.Ordinal);
    }
    
    public override int GetHashCode()
    {
        return StringComparer.Ordinal.GetHashCode(Code);
    }

    public static IEnumerable<CurrencyInfo> GenerateCurrencyList()
    {
        return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(ci => ci.Name)
            .Distinct(StringComparer.Ordinal)
            .Select(id => new RegionInfo(id))
            .GroupBy(r => r.ISOCurrencySymbol, StringComparer.Ordinal)
            .Select(g => g.First())
            .Select(r => new CurrencyInfo(r.ISOCurrencySymbol, r.CurrencyEnglishName, r.CurrencySymbol))
            .Where(r => !string.IsNullOrWhiteSpace(r.Name));
    }
}