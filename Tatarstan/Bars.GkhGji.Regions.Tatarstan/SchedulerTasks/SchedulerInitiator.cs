namespace Bars.GkhGji.Regions.Tatarstan.SchedulerTasks
{
    using Bars.B4.Events;
    using Bars.B4.Modules.Quartz;

    using global::Quartz.Impl;
    using global::Quartz.Impl.Triggers;

    /// <summary>
    /// Обработчик события запуска приложения. Инициализатор задач планировщика
    /// </summary>
    public class SchedulerInitiator : EventHandlerBase<AppStartEventArgs>
    {
        public override void OnEvent(AppStartEventArgs args)
        {
            var tr = new CronTriggerImpl("UpdateRapidResponseSystemApealStateTrigger", "RapidResponseSystem", "0 01 00 * * ?");

            var jobs = new JobDetailImpl("UpdateRapidResponseSystemApealState", "RapidResponseSystem", typeof(TaskJob<UpdateRapidResponseSystemApealStateTask>));

            jobs.Schedule(tr);
        }
    }
}