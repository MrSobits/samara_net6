namespace Bars.GkhGji.Regions.Tatarstan.Quartz
{
    using System;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Events;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using DomainService;
    using global::Quartz.Impl;
    using global::Quartz.Impl.Triggers;
    using Impl;

    public class InitQuartz : EventHandlerBase<AppEventArgsBase>
    {

        public override void OnEvent(AppEventArgsBase args)
        {
#if !DEBUG
            var config = ApplicationContext.Current.Container.Resolve<IGjiTatParamService>().GetConfig();

            ResolutionReminder();

            if (config.GetAs<bool>("GisGmpEnable"))
            {
                GisCharge();
                GisPayment();
            }
#endif
        }

        private void ResolutionReminder()
        {
            var trigger = new SimpleTriggerImpl(
                "ReminderQuartz",
                "ResolutionGroup",
                DateTime.UtcNow,
                null,
                SimpleTriggerImpl.RepeatIndefinitely,
                TimeSpan.FromMinutes(60));

            var job = new JobDetailImpl("ReminderQuartz", "ResolutionGroup", typeof(TaskJob<ResolutionQuartzTask>));
            job.Schedule(trigger);
        }

        private void GisCharge()
        {
            var trigger = new SimpleTriggerImpl(
                "GisChargeQuartz",
                "GisCharge",
                DateTime.UtcNow,
                null,
                SimpleTriggerImpl.RepeatIndefinitely,
                TimeSpan.FromMinutes(15));

            var job = new JobDetailImpl("GisChargeQuartz", "GisCharge", typeof(TaskJob<GisChargeTask>));
            job.Schedule(trigger);
        }

        private void GisPayment()
        {
            var timeConfig =
                ApplicationContext.Current.Container.Resolve<IConfigProvider>()
                    .GetConfig()
                    .GetModuleConfig("Bars.GkhGji.Regions.Tatarstan")
                    .GetAs<string>("GisGmpLoadTime");

            if (string.IsNullOrEmpty(timeConfig))
            {
                //не указано время запуска
                return;
            }

            var times = timeConfig.Contains(",")
                ? timeConfig.Split(",")
                : new[] {timeConfig};

            var now = DateTime.Today;

            for (int i = 0; i < times.Length; i++)
            {
                var time = times[i].Split(":");

                if (time.Length != 3)
                {
                    continue;
                }

                var hour = time[0].ToInt();
                var minute = time[1].ToInt();
                var second = time[2].ToInt();

                if (hour < 0 || hour > 23
                    || minute < 0 || minute > 59
                    || second < 0 || second > 59)
                {
                    continue;
                }

                var dateStart = new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    hour,
                    minute,
                    second);

                var trigger = new SimpleTriggerImpl(
                    "GisPaymentQuartz" + i,
                    "GisPayment",
                    dateStart,
                    null,
                    SimpleTriggerImpl.RepeatIndefinitely,
                    TimeSpan.FromDays(1));

                var job = new JobDetailImpl("GisPaymentQuartz" + i, "GisPayment", typeof (TaskJob<GisPaymentTask>));
                job.Schedule(trigger);
            }
        }
    }
}