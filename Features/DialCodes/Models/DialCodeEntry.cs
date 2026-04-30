using System.Text.Json.Serialization;

namespace ZoaReference.Features.DialCodes.Models;

public record DialCodeEntry(
    [property: JsonPropertyName("sheet")] string Sheet,
    [property: JsonPropertyName("label")] string Label,
    [property: JsonPropertyName("code")] string Code
);
