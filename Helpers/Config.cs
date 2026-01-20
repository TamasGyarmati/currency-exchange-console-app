namespace CurrencyApp.Helpers;

public static class Config
{
    public static readonly string? key = Environment.GetEnvironmentVariable("CURRENCY_API_KEY");
    public const string cacheFile = "currencyCache.json";
    public const string baseForCurrency = "USD";
    public const string outputType = "JSON";
    public const int cacheDurationMinutes = 60;
    public static string GetApiUrl() 
        => $"https://currencyapi.net/api/v1/rates?key={key}&base={baseForCurrency}&output={outputType}";
}