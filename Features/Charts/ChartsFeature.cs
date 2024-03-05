using Coravel.Scheduling.Schedule.Interfaces;
using ZmaReference.Features.Charts.ScheduledJobs;
using ZmaReference.FeatureUtilities.Interfaces;
using ZmaReference.Features.Charts.Services;

namespace ZmaReference.Features.Charts;

public class ChartsFeature : IServiceConfigurator, ISchedulerConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<AviationApiChartService>();
        services.AddTransient<FetchAndCacheCharts>();
        return services;
    }

    public Action<IScheduler> ConfigureScheduler()
    {
        var rnd = new Random();
        return scheduler =>
        {
            scheduler.Schedule<FetchAndCacheCharts>()
                .HourlyAt(rnd.Next(60))
                .RunOnceAtStart();
        };
    }
}
