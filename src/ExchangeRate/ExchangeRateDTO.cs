namespace ExchangeRate;

/// <summary>
/// <a href="https://www.exchangerate-api.com">Rates By Exchange Rate API</a>
/// </summary>
public sealed class ExchangeRateDto
{
    public string Result { get; set; }
    public string Provider { get; set; }
    public string Documentation { get; set; }
    public string TermsOfUse { get; set; }
    public long TimeLastUpdateUnix { get; set; }
    public string TimeLastUpdateUtc { get; set; }
    public long TimeNextUpdateUnix { get; set; }
    public string TimeNextUpdateUtc { get; set; }
    public long TimeEolUnix { get; set; }
    public string BaseCode { get; set; }
    public Dictionary<string, double> Rates { get; set; } = new(StringComparer.Ordinal);
}