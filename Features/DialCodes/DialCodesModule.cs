using Coravel.Scheduling.Schedule.Interfaces;
using ZoaReference.Features.DialCodes.Repositories;
using ZoaReference.Features.DialCodes.ScheduledJobs;
using ZoaReference.FeatureUtilities.Interfaces;

namespace ZoaReference.Features.DialCodes;

public class DialCodesModule : IServiceConfigurator, ISchedulerConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<DialCodeRepository>();
        services.AddTransient<FetchAndStoreDialCodes>();
        return services;
    }

    public Action<IScheduler> ConfigureScheduler()
    {
        var rnd = new Random();
        return scheduler =>
        {
            scheduler.Schedule<FetchAndStoreDialCodes>()
                .HourlyAt(rnd.Next(60))
                .RunOnceAtStart();
        };
    }
}
