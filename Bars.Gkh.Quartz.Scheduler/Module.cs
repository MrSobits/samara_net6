namespace Bars.Gkh.Quartz.Scheduler
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.Gkh.Quartz.Scheduler.Listeners;

    using global::Quartz;
    using global::Quartz.Spi;

    using NLog;

    using Component = Castle.MicroKernel.Registration.Component;

    /// <summary>
    /// Класс для инициализации модуля 
    /// </summary>
    public class Module : AssemblyDefinedModule
    {
        /// <summary>
        /// Объект логировщика
        /// </summary>
        static readonly Logger Logger = LogManager.GetLogger(typeof(Module).FullName);

        /// <summary>
        /// Метод регистрации компонентов в контейнере
        /// </summary>
        public override void Install()
        {
            this.RegistrationQuartz();
        }

        public void RegistrationQuartz()
        {
            // Регистрация только для веб-приложений
            if (ApplicationContext.Current.GetType().FullName == "Bars.B4.WebAppContext")
            {
                Component
                    .For<ISchedulerFactory>()
                    .ImplementedBy<TaskSchedulerFactory>()
                    .Named("TaskSchedulerFactory")
                    .LifestyleSingleton()
                    .RegisterIn(this.Container);

                Component
                    .For<ITriggerListener>()
                    .ImplementedBy<TriggerEventsHandler>()
                    .Named("TriggerEventsHandler")
                    .LifestyleTransient()
                    .RegisterIn(this.Container);

                Component.For<IScheduler>()
                    .Named("TaskScheduler")
                    .UsingFactoryMethod(
                    (k, cc) =>
                    {
                        var scheduler = k.Resolve<ISchedulerFactory>("TaskSchedulerFactory").GetScheduler();
                        scheduler.JobFactory = k.Resolve<IJobFactory>("TaskFactory");

                        var triggerEventsHandler = k.Resolve<ITriggerListener>("TriggerEventsHandler");
                        scheduler.ListenerManager.AddTriggerListener(triggerEventsHandler);

                        return scheduler;
                    })
                    .LifestyleSingleton()
                    .RegisterIn(this.Container);

                Component.For<IJobFactory>()
                    .ImplementedBy<TaskFactory>()
                    .Named("TaskFactory")
                    .LifestyleSingleton()
                    .RegisterIn(this.Container);

                var events = this.Container.Resolve<IEventAggregator>();

                events.GetEvent<AppStartEvent>().Subscribe(
                    x =>
                    {
                        try
                        {
                            this.Container.Resolve<IScheduler>("TaskScheduler").Start();
                        }
                        catch (Exception exception)
                        {
                            Module.Logger.Error(exception, "TaskScheduler start error");
                        }
                    });

                events.GetEvent<AppStopEvent>().Subscribe(
                    x =>
                    {
                        try
                        {
                            this.Container.Resolve<IScheduler>("TaskScheduler").Shutdown();
                        }
                        catch (Exception exception)
                        {
                            Module.Logger.Error(exception, "TaskScheduler shutdown error");
                        }
                    });
            }
        }
    }
}
