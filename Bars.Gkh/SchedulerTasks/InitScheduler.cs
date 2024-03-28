namespace Bars.Gkh.SchedulerTasks
{
    using Bars.B4.Events;
    using Bars.B4.Modules.Quartz;
    using global::Quartz.Impl;
    using global::Quartz.Impl.Triggers;

    public class InitScheduler : EventHandlerBase<AppStartEventArgs>
    {
        public override void OnEvent(AppStartEventArgs args)
        {
            var tr = new CronTriggerImpl("UpdateRoContractsTrigger", "UpdateRoContracts", "0 0 1 * * ?");

            var jobs = new JobDetailImpl("UpdateRoContractsJob", "UpdateRoContracts", typeof(TaskJob<UpdateRoContractTask>));

            jobs.Schedule(tr);
        }
    }
}