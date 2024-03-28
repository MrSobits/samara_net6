namespace Bars.Gkh.Reforma.Tasks
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.Domain;

    using global::Quartz;

    public class InitScheduler : EventHandlerBase<AppStartEventArgs>
    {
        public override void OnEvent(AppStartEventArgs args)
        {
            IDataResult result = null;
            var container = ApplicationContext.Current.Container;

            container.UsingForResolved<ISyncService>((c, service) => result = service.GetParams());
            if (!result.Success)
            {
                return;
            }

            var settings = (Dictionary<string, object>)result.Data;

            if (!settings.Get("Enabled").ToBool())
            {
                return;
            }

            var startTime = settings.Get("IntegrationTime").ToDateTime();
            container.Resolve<IScheduler>()
                .ScheduleJob(
                    JobBuilder.Create<TaskJob<ReformaIntegrationTask>>().WithIdentity("Integration", "Reforma").Build(),
                    TriggerBuilder.Create()
                        .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(TimeOfDay.HourMinuteAndSecondOfDay(startTime.Hour, startTime.Minute, startTime.Second)).WithIntervalInHours(24))
                        .StartNow()
                        .WithIdentity("Integration", "Reforma")
                        .Build());
        }
    }
}