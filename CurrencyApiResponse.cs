using System.Text.Json.Serialization;

namespace CurrencyApp;

public sealed record CurrencyApiResponse(
    [property: JsonPropertyName("valid")] bool Valid,
    [property: JsonPropertyName("updated")] long Updated,
    [property: JsonPropertyName("base")] string Base,
    [property: JsonPropertyName("rates")] Dictionary<string, decimal> Rates
);

public sealed record CachedCurrency(
    DateTime Timestamp,
    CurrencyApiResponse Data
);