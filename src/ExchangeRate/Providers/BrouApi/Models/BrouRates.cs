using System.Text.Json.Serialization;

namespace ExchangeRate.Providers.BrouApi.Models;

public class BrouRates
{
    [JsonPropertyName("dolar")]
    public BrouCurrencyRate Dolar { get; set; }
    
    [JsonPropertyName("dolar_ebrou")]
    public BrouCurrencyRate DolarEbrou { get; set; }
    
    [JsonPropertyName("euro")]
    public BrouCurrencyRate Euro { get; set; }
    
    [JsonPropertyName("peso_argentino")]
    public BrouCurrencyRate PesoArgentino { get; set; }
    
    [JsonPropertyName("real")]
    public BrouCurrencyRate Real { get; set; }
    
    [JsonPropertyName("libra_esterlina")]
    public BrouCurrencyRate LibraEsterlina { get; set; }
    
    [JsonPropertyName("franco_suizo")]
    public BrouCurrencyRate FrancoSuizo { get; set; }
    
    [JsonPropertyName("guarani")]
    public BrouCurrencyRate Guarani { get; set; }
    
    [JsonPropertyName("unidad_indexada")]
    public BrouCurrencyRate UnidadIndexada { get; set; }
    
    [JsonPropertyName("onza_troy_de_oro")]
    public BrouCurrencyRate OnzaTroyDeOro { get; set; }
    
    public IEnumerable<BrouCurrencyRate> GetAllRates()
    {
        yield return Dolar with { CurrencyName = "USD" };
        yield return DolarEbrou with { CurrencyName = "USD_EBROU" };
        yield return Euro with { CurrencyName = "EUR" };
        yield return PesoArgentino with { CurrencyName = "ARS" };
        yield return Real with { CurrencyName = "BRL" };
        yield return LibraEsterlina with { CurrencyName = "GBP" };
        yield return FrancoSuizo with { CurrencyName = "CHF" };
        yield return Guarani with { CurrencyName = "PYG" };
        yield return UnidadIndexada with { CurrencyName = "UNIDAD_INDEXADA" };
        yield return OnzaTroyDeOro with { CurrencyName = "XAU" };
    }
}