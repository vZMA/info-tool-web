using Coravel.Scheduling.Schedule.Interfaces;
using ZmaReference.Features.Docs.Repositories;
using ZmaReference.Features.Docs.ScheduledJobs;
using ZmaReference.FeatureUtilities.Interfaces;

namespace ZmaReference.Features.Docs;

public class DocsModule : IServiceConfigurator, ISchedulerConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<DocumentRepository>();
        services.AddTransient<FetchAndStoreZmaDocs>();
        return services;
    }

    public Action<IScheduler> ConfigureScheduler()
    {
        return scheduler =>
        {
            scheduler.Schedule<FetchAndStoreZmaDocs>()
                .Hourly()
                .RunOnceAtStart();
        };
    }
}
