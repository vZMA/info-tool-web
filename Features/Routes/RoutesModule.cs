using Coravel.Scheduling.Schedule.Interfaces;
using ZmaReference.Features.Routes.Repositories;
using ZmaReference.Features.Routes.ScheduledJobs;
using ZmaReference.Features.Routes.Services;
using ZmaReference.FeatureUtilities.Interfaces;

namespace ZmaReference.Features.Routes;

public class RoutesModule : IServiceConfigurator, ISchedulerConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<FlightAwareRouteService>();
        services.AddSingleton<AliasRouteRuleRepository>();
        services.AddSingleton<LoaRuleRepository>();
        services.AddTransient<FetchAndStoreAliasRoutes>();
        services.AddTransient<FetchAndStoreLoaRules>();
        return services;
    }
    
    public Action<IScheduler> ConfigureScheduler()
    {
        var rnd = new Random();
        return scheduler =>
        {
            scheduler.Schedule<FetchAndStoreAliasRoutes>()
                .HourlyAt(rnd.Next(60))
                .RunOnceAtStart();
            
            scheduler.Schedule<FetchAndStoreLoaRules>()
                .HourlyAt(rnd.Next(60))
                .RunOnceAtStart();
        };
    }
}
