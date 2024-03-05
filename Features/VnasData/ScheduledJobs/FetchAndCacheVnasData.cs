using Coravel.Invocable;
using Microsoft.Extensions.Options;
using ZmaReference.Features.Charts.ScheduledJobs;
using ZmaReference.Features.Charts.Services;
using ZmaReference.Features.VnasData.Services;

namespace ZmaReference.Features.VnasData.ScheduledJobs;

public class FetchAndCacheVnasData(ILogger<FetchAndCacheCharts> logger, CachedVnasDataService vnasDataService) : IInvocable
{
    public async Task Invoke()
    {
        // This method forces service to fetch and cache, and we can discard value
        logger.LogInformation("Fetching all VNAS Data for ZMA");
        await vnasDataService.ForceCache("ZMA");
    }
}