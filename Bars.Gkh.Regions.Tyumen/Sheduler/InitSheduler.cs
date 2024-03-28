namespace Bars.Gkh.Regions.Tyumen.Sheduler
{
    using B4.Events;
    using B4.Modules.Quartz;
    using global::Quartz;
    using global::Quartz.Impl;
    using Tasks;

    public class InitSheduler : EventHandlerBase<AppStartEventArgs>
    {
        public override void OnEvent(AppStartEventArgs args)
        {
            this.CheckExpireSuggestionWithTerm();
        }

        private void CheckExpireSuggestionWithTerm()
        {
            var trigger = TriggerBuilder.Create()
                .WithIdentity("CloseExpireSuggestionWithTerm", "SuggestionGroup")
                .StartAt(DateBuilder.TodayAt(23, 30, 0))
                .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 30))).Build();

            var job = new JobDetailImpl("CloseExpireSuggestionWithTerm", "SuggestionGroup", typeof(TaskJob<CloseExpireSuggestionWithTermTask>));
            job.Schedule(trigger);
        }
    }
}
