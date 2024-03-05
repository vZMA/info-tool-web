using Coravel.Scheduling.Schedule.Interfaces;
using ZmaReference.FeatureUtilities.Interfaces;
using ZmaReference.Features.VnasData.ScheduledJobs;
using ZmaReference.Features.VnasData.Services;

namespace ZmaReference.Features.VnasData;

public class VnasDataFeature : IServiceConfigurator, ISchedulerConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<CachedVnasDataService>();
        services.AddTransient<FetchAndCacheVnasData>();
        return services;
    }

    public Action<IScheduler> ConfigureScheduler()
    {
        var rnd = new Random();
        return scheduler =>
        {
            scheduler.Schedule<FetchAndCacheVnasData>()
                .HourlyAt(rnd.Next(60))
                .RunOnceAtStart();
        };
    }
}
