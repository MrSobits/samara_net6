namespace Bars.Gkh.DomainEvent.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Application;
    using Bars.B4.IoC;

    using Castle.Windsor;

    /// <summary>
    /// Класс для регистрации, возбуждения доменных событий в приложении
    /// </summary>
    public class DomainEvents
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static List<Delegate> actions;

        private static IWindsorContainer Container
        {
            get
            {
                return ApplicationContext.Current.Container;
            }
        }

        /// <summary>
        /// Регистрация делегата обработчика для доменного события
        /// </summary>
        /// <typeparam name="T">Тип события</typeparam>
        /// <param name="callback">Делегат-обработчик события</param>
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (DomainEvents.actions == null)
                DomainEvents.actions = new List<Delegate>();

            DomainEvents.actions.Add(callback);
        }

        /// <summary>
        /// Очищает список зарегистрированных делегатов-обработчиков
        /// </summary>
        public static void ClearCallbacks()
        {
            DomainEvents.actions = null;
        }

        /// <summary>
        /// Отправляет событие обработчикам
        /// </summary>
        /// <typeparam name="T">Тип события</typeparam>
        /// <param name="args">Аргументы</param>
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (DomainEvents.Container != null)
            {
                var handlers = DomainEvents.Container.ResolveAll<IDomainEventHandler<T>>();

                using (DomainEvents.Container.Using(handlers))
                {
                    foreach (var handler in handlers)
                        handler.Handle(args);
                }
            }

            if (DomainEvents.actions != null)
            {
                foreach (var action in DomainEvents.actions)
                    if (action is Action<T>)
                        ((Action<T>) action)(args);
            }
        }
    }
}