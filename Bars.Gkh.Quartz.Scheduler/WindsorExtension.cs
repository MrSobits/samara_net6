namespace Bars.Gkh.Quartz.Scheduler
{
    using System;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using global::Quartz;

    /// <summary>
    /// Расширения для IoC контейнера
    /// </summary>
    public static class WindsorExtension
    {
        /// <summary>
        /// Метод регистрации в контейнере задачи
        /// </summary>
        /// <typeparam name="T">Тип задачи</typeparam>
        /// <param name="container">Экземпляр контейнера</param>
        public static void RegisterTask<T>(this IWindsorContainer container) where T : IJob
        {
            container.RegisterTask(typeof(T));
        }

        /// <summary>
        /// Метод регистрации в контейнере задачи
        /// </summary>
        /// <param name="container">Экземпляр контейнера</param>
        /// <param name="taskType">Тип задачи</param>
        public static void RegisterTask(this IWindsorContainer container, Type taskType)
        {
            container.Register(Component.For<IJob>().Named(taskType.Name).ImplementedBy(taskType).LifeStyle.Transient);
        }
    }
}
