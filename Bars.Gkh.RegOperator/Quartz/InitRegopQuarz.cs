namespace Bars.Gkh.RegOperator.Quartz
{
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Quartz;

    using global::Quartz;
    using global::Quartz.Impl;

    /// <summary>
    /// Инициализатор фоновых задач РегОператора
    /// </summary>
    public class InitRegopQuarz : EventHandlerBase<AppStartEventArgs>
    {
        private bool enablePersonalStateChange;

        /// <summary>
        /// Метод, вызываем при обработке события
        /// </summary>
        /// <param name="args">Аргументы события</param>
        public override void OnEvent(AppStartEventArgs args)
        {
            ApplicationContext.Current.Container.UsingForResolved<IConfigProvider>((container, service) => 
                this.enablePersonalStateChange = service.GetConfig().AppSettings.GetAs("RegOperator.ChangePersonalAccountState.Enabled", true));

            this.UnactivatePersonalAccount();

            this.ActivateDecisionProtocol();

            // убрали планировку, запускаем автоматически после ActivateDecisionProtocol (вручную добавляем в планировщик в самой задаче ActivateDecisionProtocol)
            //UpdateConditionHouseAndPersAccs(); 
        }

        private void UnactivatePersonalAccount()
        {
            if (!this.enablePersonalStateChange)
            {
                return;
            }

            var trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("UnactivatePersonalAccount", "RegopGroup")
                .StartAt(DateBuilder.TomorrowAt(2, 0, 0))
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever()).Build();

            var job = new JobDetailImpl("UnactivatePersonalAccount", "RegopGroup", typeof(TaskJob<UnactivatePersonalAccountTask>));
            job.Schedule(trigger);
        }

        private void ActivateDecisionProtocol()
        {
            if (!this.enablePersonalStateChange)
            {
                return;
            }

            var trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("ActivateDecisionProtocol", "RegopGroup")
                .StartAt(DateBuilder.TomorrowAt(1, 0, 0))
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever()).Build();

            var job = new JobDetailImpl("ActivateDecisionProtocol", "RegopGroup", typeof(TaskJob<ActivateDecisionProtocolTask>));
            job.Schedule(trigger);
        }

        private void UpdateConditionHouseAndPersAccs()
        {
            if (!this.enablePersonalStateChange)
            {
                return;
            }

            var trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("UpdateConditionHouseAndPersAccs", "RegopGroup")
                .StartAt(DateBuilder.TomorrowAt(1, 0, 0))
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever()).Build();

            var job = new JobDetailImpl("UpdateConditionHouseAndPersAccs", "RegopGroup", typeof(TaskJob<UpdateConditionHouseAndPersAccsTask>));
            job.Schedule(trigger);
        }
    }
}
