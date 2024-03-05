using Coravel.Scheduling.Schedule.Interfaces;
using ZmaReference.Features.IcaoReference.Repositories;
using ZmaReference.Features.IcaoReference.ScheduledJobs;
using ZmaReference.FeatureUtilities.Interfaces;

namespace ZmaReference.Features.IcaoReference;

public class IcaoReferenceModule : IServiceConfigurator, ISchedulerConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<AirlineRepository>();
        services.AddSingleton<AircraftTypeRepository>();
        services.AddSingleton<AirportRepository>();
        services.AddTransient<FetchAndStoreAircraftInfo>();
        services.AddTransient<FetchAndStoreAirlineIcao>();
        services.AddTransient<FetchAndStoreAirports>();
        return services;
    }
    
    public Action<IScheduler> ConfigureScheduler()
    {
        var rnd = new Random();
        return scheduler =>
        {
            scheduler.Schedule<FetchAndStoreAircraftInfo>()
                .DailyAt(7, rnd.Next(60))
                .RunOnceAtStart();
            
            scheduler.Schedule<FetchAndStoreAirlineIcao>()
                .DailyAt(7, rnd.Next(60))
                .RunOnceAtStart();
            
            scheduler.Schedule<FetchAndStoreAirports>()
                .DailyAt(7, rnd.Next(60))
                .RunOnceAtStart();
        };
    }
}
