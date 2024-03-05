using Coravel.Scheduling.Schedule.Interfaces;
using ZmaReference.Features.DigitalAtis.Repositories;
using ZmaReference.Features.DigitalAtis.ScheduledJobs;
using ZmaReference.FeatureUtilities.Interfaces;

namespace ZmaReference.Features.DigitalAtis;

public class DigitalAtisModule : IServiceConfigurator, ISchedulerConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<DigitalAtisRepository>();
        services.AddTransient<FetchAndStoreAtis>();
        return services;
    }

    public Action<IScheduler> ConfigureScheduler()
    {
        return scheduler =>
        {
            scheduler.Schedule<FetchAndStoreAtis>()
                .EveryMinute()
                .RunOnceAtStart();
        };
    }
}
