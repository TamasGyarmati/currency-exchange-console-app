using System.Text.Json;

namespace CurrencyApp;

public enum ConversionType
{
    EURtoHUF = 1,
    HUFtoEUR = 2
}

public class GetCurrencyData
{
    const string cacheFile = "currencyCache.json";
    const string key = "f7809add3fb3c9d8f8fabd5e7331e07a89d8";
    const string baseForCurrency = "USD";
    const string outputType = "JSON";
    const string apiUrl = $"https://currencyapi.net/api/v1/rates?key={key}&base={baseForCurrency}&output={outputType}";
    const int cacheDurationMinutes = 60;
    
    public static async Task GetData(ConversionType conversionType)
    {
        CurrencyApiResponse? data = null;

        Console.WriteLine("Amount: ");
        int szam = int.Parse(Console.ReadLine() ?? string.Empty);
        
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

            decimal eurToHufRate = usdToHuf / usdToEur;
            decimal hufToEurRate = 1 / eurToHufRate;
            
            switch (conversionType)
            {
                case ConversionType.EURtoHUF:
                    decimal eurToHuf = szam * eurToHufRate;
                    Console.WriteLine($"In HUF: {eurToHuf:F2} Ft");
                    break;
                case ConversionType.HUFtoEUR:
                    decimal hufToEur = szam * hufToEurRate;
                    Console.WriteLine($"In EUR: €{hufToEur:F2}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(conversionType), conversionType, null);
            }
        }
    }
}