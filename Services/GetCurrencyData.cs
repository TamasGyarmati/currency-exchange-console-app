using System.Text.Json;
using CurrencyApp.Helpers;
using CurrencyApp.Models;

namespace CurrencyApp.Services;

public enum ConversionType
{
    EURtoHUF = 1,
    HUFtoEUR = 2
}

public static class GetCurrencyData
{
    /// <summary>
    /// Retrieves the current EUR↔HUF exchange rates from the currencyapi.net API (or from the cache if valid),
    /// then converts a user-specified amount between EUR and HUF.
    /// </summary>
    /// <remarks>
    /// The method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>
    /// Prompts the user to enter a positive numeric amount via the console.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Retrieves exchange rate data either from a valid local cache file
    /// (<c>Config.cacheFile</c>) or, if the cache is missing or expired,
    /// fetches fresh data from the currencyapi.net API and stores it locally.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Converts the specified amount between EUR and HUF based on the selected
    /// <see cref="ConversionType"/> and prints the result to the console.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="type">
    /// Defines the conversion direction:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="ConversionType.EURtoHUF"/> – Convert from Euro (EUR) to Hungarian Forint (HUF).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="ConversionType.HUFtoEUR"/> – Convert from Hungarian Forint (HUF) to Euro (EUR).
    /// </description>
    /// </item>
    /// </list>
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when an unsupported <see cref="ConversionType"/> value is provided.
    /// </exception>
    public static async Task CurrencyCalculator(ConversionType type)
    {
        decimal amount = CheckAndGetUserInput(type);
        var data = await CheckIfCachedFileExistAndTimestampNotExpired() ?? await FetchDataAndCache();
        CurrencyConversion(data, type, amount);
    }

    private static async Task<CurrencyApiResponse?> CheckIfCachedFileExistAndTimestampNotExpired()
    {
        if (File.Exists(Config.cacheFile))
        {
            string cachedText = await File.ReadAllTextAsync(Config.cacheFile);
            try
            {
                var cachedObject = JsonSerializer.Deserialize<CachedCurrency>(cachedText);
                if (cachedObject != null && (DateTime.Now - cachedObject.Timestamp).TotalMinutes < Config.cacheDurationMinutes)
                {
                    return cachedObject.Data;
                }
            }
            catch
            {
                File.Delete(Config.cacheFile);
            }
        }
        return null;
    }
    
    private static async Task<CurrencyApiResponse> FetchDataAndCache()
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(Config.GetApiUrl());
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<CurrencyApiResponse>(json);

        if (data is not null)
        {
            var cached = new CachedCurrency(DateTime.Now, data);
            var cacheJson = JsonSerializer.Serialize(cached, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(Config.cacheFile, cacheJson);
        }
        else
            throw new Exception("Something went wrong while serializing the JSON.");

        return data;
    }
    
    private static void CurrencyConversion(CurrencyApiResponse data, ConversionType type, decimal amount)
    {
        decimal usdToEur = data.Rates["EUR"];
        decimal usdToHuf = data.Rates["HUF"];

        decimal eurToHufRate = usdToHuf / usdToEur;
        decimal hufToEurRate = 1 / eurToHufRate;
            
        switch (type)
        {
            case ConversionType.EURtoHUF:
                decimal eurToHuf = amount * eurToHufRate;
                Console.WriteLine($"In HUF: {eurToHuf:F2} Ft");
                break;
            case ConversionType.HUFtoEUR:
                decimal hufToEur = amount * hufToEurRate;
                Console.WriteLine($"In EUR: €{hufToEur:F2}");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    private static decimal CheckAndGetUserInput(ConversionType type)
    {
        string label = type == ConversionType.EURtoHUF
            ? "Amount (EUR): €"
            : "Amount (HUF): Ft ";
        
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal amount) && amount > 0)
            {
                return amount;
            }
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input. Please enter a valid number (e.g. 1, 4, 2,55, 1,7976931348623157E+308).\n");
            Console.ResetColor();
        }
    }
}