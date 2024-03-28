namespace Bars.GkhEdoInteg.Quartz
{
    using System;

    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Events;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;

    using global::Quartz.Impl;
    using global::Quartz.Impl.Triggers;

    public class InitQuartz : EventHandlerBase<AppStartEventArgs>
    {
        public override void OnEvent(AppStartEventArgs args)
        {
#if !DEBUG
            var config = ApplicationContext.Current.Container.Resolve<IConfigProvider>().GetConfig();
            if (config == null || !config.AppSettings.ContainsKey("AutomaticIntegEdo"))
            {
                return;
            }

            if (!config.AppSettings["AutomaticIntegEdo"].ToBool())
            {
                return;
            }

            var automaticIntegEdoLoadDataInterval = config.AppSettings["AutomaticIntegEdoLoadDataInterval"].ToInt(60);
            var automaticIntegEdoLoadDocumentsInterval = config.AppSettings["AutomaticIntegEdoLoadDocumentsInterval"].ToInt(112);

            var trigger = new SimpleTriggerImpl(
                                         "EdoTrigger",
                                         "EdoGroup",
                               DateTime.UtcNow,
                               null,
                               SimpleTriggerImpl.RepeatIndefinitely,
                               TimeSpan.FromMinutes(automaticIntegEdoLoadDataInterval));

            var job = new JobDetailImpl("IntegEdo", "EdoGroup", typeof(TaskJob<EdoLoadAppealCitsTask>));
            job.Schedule(trigger);

            var triggerDoc = new SimpleTriggerImpl(
                                        "EdoTrigger",
                                        "EdoDocGroup",
                              DateTime.UtcNow,
                              null,
                              SimpleTriggerImpl.RepeatIndefinitely,
                              TimeSpan.FromMinutes(automaticIntegEdoLoadDocumentsInterval));

            var jobDoc = new JobDetailImpl("DocIntegEdo", "DocEdoGroup", typeof(TaskJob<DocEdoLoadAppealCitsTask>));
            jobDoc.Schedule(triggerDoc);
#endif
        }
    }
}
