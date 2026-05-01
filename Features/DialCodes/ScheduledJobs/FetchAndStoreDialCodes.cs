using Coravel.Invocable;
using Microsoft.Extensions.Options;
using ZoaReference.Features.DialCodes.Models;
using ZoaReference.Features.DialCodes.Repositories;

namespace ZoaReference.Features.DialCodes.ScheduledJobs;

public class FetchAndStoreDialCodes(
    ILogger<FetchAndStoreDialCodes> logger,
    IHttpClientFactory httpClientFactory,
    IOptionsMonitor<AppSettings> appSettings,
    DialCodeRepository repository)
    : IInvocable
{
    public async Task Invoke()
    {
        var url = appSettings.CurrentValue.Urls.DialCodesJson;
        if (string.IsNullOrWhiteSpace(url))
        {
            logger.LogWarning("DialCodesJson URL is not configured; skipping fetch");
            return;
        }

        try
        {
            logger.LogInformation("Starting dial codes fetch and update task");
            using var httpClient = httpClientFactory.CreateClient();
            var entries = await httpClient.GetFromJsonAsync<List<DialCodeEntry>>(url);

            if (entries is null)
            {
                logger.LogWarning("Error while fetching dial codes: null JSON deserialization from {url}", url);
                return;
            }

            repository.ReplaceAll(entries);
            logger.LogInformation("Loaded {count} dial code entries from {url}", entries.Count, url);
        }
        catch (Exception e)
        {
            logger.LogWarning("Exception while trying to fetch and update dial codes: {ex}", e);
        }
    }
}
