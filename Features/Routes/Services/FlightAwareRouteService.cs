using System.Text.RegularExpressions;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ZmaReference.Common;
using ZmaReference.Features.Routes.Models;

namespace ZmaReference.Features.Routes.Services;

public partial class FlightAwareRouteService(IHttpClientFactory httpClientFactory, IOptionsMonitor<AppSettings> appSettings, IMemoryCache cache, ILogger<FlightAwareRouteService> logger)
{

    }

    public string MakeUrl(string departureIcao, string arrivalIcao)
    {
        return appSettings.CurrentValue.Urls.FlightAwareIfrRouteBase + "origin=" + departureIcao + "&destination=" + arrivalIcao;
    }

    private static bool TryParseMinAltitude(string altitudeRange, out int minAltitude)
    {
        var match = AltitudeRangeRegex().Match(altitudeRange);
        var parseString = match.Success ? match.Groups[1].Value : altitudeRange;
        return Helpers.TryParseAltitude(parseString, out minAltitude);
    }

    private static bool TryParseMaxAltitude(string altitudeRange, out int maxAltitude)
    {
        var match = AltitudeRangeRegex().Match(altitudeRange);
        var parseString = match.Success ? match.Groups[2].Value : altitudeRange;
        return Helpers.TryParseAltitude(parseString, out maxAltitude);
    }

    private static bool TryParseDistance(string distanceStr, out int distance)
    {
        var match = DistanceRegex().Match(distanceStr);
        var parseString = match.Success ? match.Groups[1].Value.Replace(",", string.Empty) : distanceStr;
        return int.TryParse(parseString, out distance);
    }

    private static (string, string) MakeCacheKey(string departureIcao, string arrivalIcao)
    {
        return ($"FlightAwareDeparture:{departureIcao.ToUpper()}", $"FlightAwareArrival:{arrivalIcao.ToUpper()}");
    }

    private static string ParseId(string id) => id.Length > 4 ? id[^4..] : id;

    [GeneratedRegex(@"([\s\S]+) - ([\s\S]+)")]
    private static partial Regex AltitudeRangeRegex();

    [GeneratedRegex("([0-9,]+) sm")]
    private static partial Regex DistanceRegex();
}