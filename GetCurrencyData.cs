using System.Text.Json;

namespace CurrencyApp;

public class GetCurrencyData
{
    const string cacheFile = "currencyCache.json";
    const string key = "f7809add3fb3c9d8f8fabd5e7331e07a89d8";
    const string baseForCurrency = "USD";
    const string outputType = "JSON";
    const string apiUrl = $"https://currencyapi.net/api/v1/rates?key={key}&base={baseForCurrency}&output={outputType}";
    const int cacheDurationMinutes = 60;
    
    public static async Task GetData()
    {
        CurrencyApiResponse? data = null;
        
        if (File.Exists(cacheFile))
        {
            string cachedText = await File.ReadAllTextAsync(cacheFile);
            try
            {
                var cachedObject = JsonSerializer.Deserialize<CachedCurrency>(cachedText);
                if (cachedObject != null && (DateTime.Now - cachedObject.Timestamp).TotalMinutes < cacheDurationMinutes)
                {
                    data = cachedObject.Data;
                }
            }
            catch
            {
                File.Delete(cacheFile);
            }
        }
        
        if (data is null)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            data = JsonSerializer.Deserialize<CurrencyApiResponse>(json);

            if (data is not null)
            {
                var cached = new CachedCurrency(DateTime.Now, data);
                var cacheJson = JsonSerializer.Serialize(cached, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(cacheFile, cacheJson);
            }
        }

        if (data is not null)
        {
            decimal usdToEur = data.Rates["EUR"];
            decimal usdToHuf = data.Rates["HUF"];

            decimal eurToHuf = usdToHuf / usdToEur;
            Console.WriteLine($"1 EUR = {eurToHuf} HUF");
            Console.WriteLine($"1 USD = {usdToHuf} USD");   
        }
    }
}