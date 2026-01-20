using System.Text.Json;
using CurrencyApp.Helpers;
using CurrencyApp.Models;

namespace CurrencyApp.Services;

public enum ConversionType
{
    EURtoHUF = 1,
    HUFtoEUR = 2
}

public class GetCurrencyData
{
    /// <summary>
    /// Retrieves the current EUR↔HUF exchange rates from the currencyapi.net API (or from the cache if valid),
    /// then converts a user-specified amount between EUR and HUF.
    /// </summary>
    /// <param name="conversionType">
    /// Specifies the conversion direction:
    /// <list type="bullet">
    /// <item><description>ConversionType.EURtoHUF - Convert from Euro to Hungarian Forint</description></item>
    /// <item><description>ConversionType.HUFtoEUR - Convert from Hungarian Forint to Euro</description></item>
    /// </list>
    /// </param>
    /// <remarks>
    /// The method first checks if a valid cache exists in the local file (<c>Config.cacheFile</c>), 
    /// which is valid for 60 minutes (<c>Config.cacheDurationMinutes</c>). 
    /// If no valid cache is found, it sends an HTTP GET request to the API to fetch the latest rates.
    /// The user is prompted in the console to enter the amount to convert, and the method calculates 
    /// and prints the converted value rounded to two decimal places.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="conversionType"/> is not one of the defined enum values.
    /// </exception>
    public static async Task CurrencyCalculator(ConversionType conversionType)
    {
        CurrencyApiResponse? data = null;

        Console.WriteLine("Amount: ");
        int szam = int.Parse(Console.ReadLine() ?? string.Empty);
        
        if (File.Exists(Config.cacheFile))
        {
            string cachedText = await File.ReadAllTextAsync(Config.cacheFile);
            try
            {
                var cachedObject = JsonSerializer.Deserialize<CachedCurrency>(cachedText);
                if (cachedObject != null && (DateTime.Now - cachedObject.Timestamp).TotalMinutes < Config.cacheDurationMinutes)
                {
                    data = cachedObject.Data;
                }
            }
            catch
            {
                File.Delete(Config.cacheFile);
            }
        }
        
        if (data is null)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(Config.GetApiUrl());
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            data = JsonSerializer.Deserialize<CurrencyApiResponse>(json);

            if (data is not null)
            {
                var cached = new CachedCurrency(DateTime.Now, data);
                var cacheJson = JsonSerializer.Serialize(cached, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(Config.cacheFile, cacheJson);
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