namespace ExchangeRate.Interfaces;

public interface ICacheStorage
{
    public void Save(IEnumerable<ConversionData> conversionRates);
    public IEnumerable<ConversionData> Load();
    public void Clear();
}