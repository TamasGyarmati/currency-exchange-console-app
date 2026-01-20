namespace CurrencyApp.Helpers;

public static class Config
{
    static readonly string key = Environment.GetEnvironmentVariable("CURRENCY_API_KEY")
        ?? throw new InvalidOperationException("Environment variable 'CURRENCY_API_KEY' was not found");
    public const string cacheFile = "currencyCache.json";
    const string baseForCurrency = "USD";
    const string outputType = "JSON";
    public const int cacheDurationMinutes = 1440;
    public static string GetApiUrl() 
        => $"https://currencyapi.net/api/v1/rates?key={key}&base={baseForCurrency}&output={outputType}";
}