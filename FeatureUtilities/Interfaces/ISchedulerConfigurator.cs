using Coravel.Scheduling.Schedule.Interfaces;

namespace ZmaReference.FeatureUtilities.Interfaces;

public interface ISchedulerConfigurator
{
    public Action<IScheduler> ConfigureScheduler();
}
