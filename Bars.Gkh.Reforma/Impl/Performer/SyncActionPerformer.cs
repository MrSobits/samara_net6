namespace Bars.Gkh.Reforma.Impl.Performer
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.Performer;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    /// <summary>
    ///     Планировщик действий синхронизации
    /// </summary>
    public class SyncActionPerformer : ISyncActionPerformer
    {
        private static readonly int ConcurrencyLevel = Math.Min(Environment.ProcessorCount * 2, 16);

        private readonly object lockObject = new object();

        #region Constructors and Destructors
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">
        ///     IoC контейнер
        /// </param>
        /// <param name="provider">
        ///     Провайдер текущей интеграции
        /// </param>
        public SyncActionPerformer(IWindsorContainer container, ISyncProvider provider)
        {
            this.Container = container;
            this.ActionQueue = new ConcurrentQueue<QueuedAction>();
            this.Provider = provider;
        }

        #endregion

        /// <summary>
        ///     Запланированное действие
        /// </summary>
        public class QueuedAction : IQueuedActionConfigurator
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Конструктор
            /// </summary>
            /// <param name="сontainer">
            ///     IoC контейнер
            /// </param>
            /// <param name="action">
            ///     Действие
            /// </param>
            /// <param name="releaseAfterUse">
            ///     Освободить после использования?
            /// </param>
            public QueuedAction(IWindsorContainer сontainer, ISyncAction action, bool releaseAfterUse)
            {
                this.Сontainer = сontainer;
                this.Action = action;
                this.ReleaseAfterUse = releaseAfterUse;
            }

            #endregion

            #region Fields

            /// <summary>
            ///     Действие
            /// </summary>
            protected readonly ISyncAction Action;

            /// <summary>
            ///     Освободить после использования?
            /// </summary>
            protected readonly bool ReleaseAfterUse;

            /// <summary>
            ///     IoC контейнер
            /// </summary>
            protected readonly IWindsorContainer Сontainer;

            /// <summary>
            ///     Колбэк
            /// </summary>
            public PerformerCallback Callback { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Исполнить действие
            /// </summary>
            /// <param name="performer">
            ///     Планировщик действий
            /// </param>
            public void Perform(ISyncActionPerformer performer)
            {
                var result = this.Action.Perform();

                if (this.ReleaseAfterUse)
                {
                    this.Сontainer.Release(this.Action);
                }

                if (this.Callback != null)
                {
                    this.Callback(result);
                }
            }

            /// <summary>
            ///     Установка колбэка
            /// </summary>
            /// <param name="callback">Колбэк</param>
            /// <param name="supressExceptions">Подавлять исключения, возникающие в ходе исполнения</param>
            /// <returns>Настройщик запланированного действия</returns>
            IQueuedActionConfigurator IQueuedActionConfigurator.WithCallback(
                PerformerCallback callback,
                bool supressExceptions)
            {
                this.Callback = supressExceptions
                                    ? (result =>
                                        {
                                            try
                                            {
                                                callback(result);
                                            }
                                            catch
                                            {
                                                // ignored
                                            }
                                        })
                                    : callback;
                return this;
            }

            /// <summary>
            ///     Установка сериализованных параметров действия
            /// </summary>
            /// <param name="parameters">Сериализованные параметры</param>
            /// <returns>Настройщик запланированного действия</returns>
            IQueuedActionConfigurator IQueuedActionConfigurator.WithJsonEncodedParameters(string parameters)
            {
                this.Action.SerializedParameters = parameters;
                return this;
            }

            /// <summary>
            ///     Установка параметров
            /// </summary>
            /// <param name="parameters">Параметры</param>
            /// <returns>Настройщик запланированного действия</returns>
            IQueuedActionConfigurator IQueuedActionConfigurator.WithParameters(object parameters)
            {
                this.Action.Parameters = parameters;
                return this;
            }

            #endregion
        }

        /// <summary>
        ///     Запланированное действие
        /// </summary>
        /// <typeparam name="TParam">
        ///     Тип параметров
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Тип результата
        /// </typeparam>
        public class QueuedAction<TParam, TResult> : QueuedAction, IQueuedActionConfigurator<TParam, TResult>
        {
            #region Constructors and Destructors

            /// <summary>
            ///     Конструктор
            /// </summary>
            /// <param name="сontainer">
            ///     IoC контейнер
            /// </param>
            /// <param name="action">
            ///     Действие
            /// </param>
            /// <param name="releaseAfterUse">
            ///     Освободить после использования?
            /// </param>
            public QueuedAction(IWindsorContainer сontainer, ISyncAction action, bool releaseAfterUse)
                : base(сontainer, action, releaseAfterUse)
            {
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Установка колбэка
            /// </summary>
            /// <param name="callback">Колбэк</param>
            /// <param name="supressExceptions">Подавлять исключения, возникающие в ходе исполнения</param>
            /// <returns>Настройщик запланированного действия</returns>
            IQueuedActionConfigurator<TParam, TResult> IQueuedActionConfigurator<TParam, TResult>.WithCallback(
                PerformerCallback<TResult> callback,
                bool supressExceptions)
            {
                return
                    (IQueuedActionConfigurator<TParam, TResult>)
                    ((IQueuedActionConfigurator)this).WithCallback(
                        result => callback((SyncActionResult<TResult>)result),
                        supressExceptions);
            }

            /// <summary>
            ///     Установка параметров
            /// </summary>
            /// <param name="parameters">Параметры</param>
            /// <returns>Настройщик запланированного действия</returns>
            IQueuedActionConfigurator<TParam, TResult> IQueuedActionConfigurator<TParam, TResult>.WithParameters(
                TParam parameters)
            {
                return
                    (IQueuedActionConfigurator<TParam, TResult>)
                    ((IQueuedActionConfigurator)this).WithParameters(parameters);
            }

            /// <summary>
            ///     Установка сериализованных параметров действия
            /// </summary>
            /// <param name="parameters">Сериализованные параметры</param>
            /// <returns>Настройщик запланированного действия</returns>
            IQueuedActionConfigurator<TParam, TResult> IQueuedActionConfigurator<TParam, TResult>.
                WithJsonEncodedParameters(string parameters)
            {
                return
                    (IQueuedActionConfigurator<TParam, TResult>)
                    ((IQueuedActionConfigurator)this).WithJsonEncodedParameters(parameters);
            }

            #endregion
        }

        private class ActionAwaiter
        {
            private readonly ConcurrentDictionary<int, SyncActionResult> actionResults = new ConcurrentDictionary<int, SyncActionResult>();

            private int actionCount;

            private readonly Action<IEnumerable<SyncActionResult>> callback;

            public ActionAwaiter(IEnumerable<IQueuedActionConfigurator> actions, Action<IEnumerable<SyncActionResult>> callback)
            {
                var queuedActions = actions.OfType<QueuedAction>().ToArray();
                actionCount = queuedActions.Length;
                this.callback = callback;

                foreach (var queuedAction in queuedActions)
                {
                    var oldCallback = queuedAction.Callback;
                    var actionKey = queuedAction.GetHashCode();

                    queuedAction.Callback = result =>
                        {
                            this.ActionCompleted(actionKey, result);
                            if (oldCallback != null)
                            {
                                oldCallback(result);
                            }
                        };
                }
            }

            private void ActionCompleted(int actionKey, SyncActionResult result)
            {
                var count = Interlocked.Decrement(ref this.actionCount);
                this.actionResults[actionKey] = result;
                if (count == 0)
                {
                    this.callback(this.actionResults.Values);
                }
            }
        }

        #region Properties

        /// <summary>
        ///     Очередь действий
        /// </summary>
        private ConcurrentQueue<QueuedAction> ActionQueue { get; set; }

        /// <summary>
        ///     Провайдер текущей сессии
        /// </summary>
        private ISyncProvider Provider { get; }

        /// <summary>
        ///     IoC контейнер
        /// </summary>
        private IWindsorContainer Container { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Добавление действия в очередь
        /// </summary>
        /// <typeparam name="TParam">
        ///     Тип параметров действия
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Тип результата действия
        /// </typeparam>
        /// <param name="action">
        ///     Экземляр действия
        /// </param>
        /// <returns>
        ///     Настройщик запланированного действия
        /// </returns>
        public IQueuedActionConfigurator<TParam, TResult> AddToQueue<TParam, TResult>(
            ISyncAction<TParam, TResult> action)
        {
            return this.AddToQueueInternal(action, false);
        }

        /// <summary>
        ///     Добавление действия в очередь
        /// </summary>
        /// <param name="action">
        ///     Экземпляр действия
        /// </param>
        /// <returns>
        ///     Настройщик запланированного действия
        /// </returns>
        public IQueuedActionConfigurator AddToQueue(ISyncAction action)
        {
            return this.AddToQueueInternal(action, false);
        }

        /// <summary>
        ///     Добавление действия в очередь
        /// </summary>
        /// <typeparam name="TAction">
        ///     Тип действия
        /// </typeparam>
        /// <returns>
        ///     Настройщик запланированного действия
        /// </returns>
        public IQueuedActionConfigurator AddToQueue<TAction>() where TAction : ISyncAction
        {
            var action = this.ResolveAction<TAction>();
            return this.AddToQueueInternal(action, true);
        }

        /// <summary>
        ///     Добавление действия в очередь
        /// </summary>
        /// <typeparam name="TAction">
        ///     Тип действия
        /// </typeparam>
        /// <typeparam name="TParam">
        ///     Тип параметров действия
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Тип результата действия
        /// </typeparam>
        /// <returns>
        ///     Настройщик запланированного действия
        /// </returns>
        public IQueuedActionConfigurator<TParam, TResult> AddToQueue<TAction, TParam, TResult>()
            where TAction : ISyncAction<TParam, TResult>
        {
            var action = this.ResolveAction<TAction>();
            return this.AddToQueueInternal(action, true);
        }

        private TAction ResolveAction<TAction>() where TAction : ISyncAction
        {
            var args = new Arguments { { typeof(ISyncProvider), this.Provider } };
            var action = this.Container.Resolve<TAction>(args);
            return action;
        }

        /// <summary>
        ///     Запуск исполнения очереди действий
        /// </summary>
        public void Perform()
        {
            var semaphore = new SemaphoreSlim(ConcurrencyLevel, ConcurrencyLevel);
            while (!this.ActionQueue.IsEmpty)
            {
                QueuedAction action;
                while (this.ActionQueue.TryDequeue(out action))
                {
                    var localAction = action;
                    semaphore.Wait();
                    Task.Factory.StartNew(
                        () =>
                        {
                            try
                            {
                                using (this.Container.BeginScope())
                                {
                                    localAction.Perform(this);
                                }
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });
                }

                // ждём всех семафоров, но т.к. задачи могут поставить в очередь новые задачи,
                // то мы ещё раз проверяем в главном цикле очередь на пустоту
                while (semaphore.CurrentCount < ConcurrencyLevel)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void WhenAll(IEnumerable<IQueuedActionConfigurator> actions, Action<IEnumerable<SyncActionResult>> callback)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ActionAwaiter(actions, callback);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Внутренний метод добавления в очередь. Помимо прочего, требует указать признак
        ///     способа создания объекта. Объекты полученные из IoC контейнера будут освобождены
        ///     после исполнения
        /// </summary>
        /// <param name="action">
        ///     Действие синхронизации
        /// </param>
        /// <param name="autoCreated">
        ///     Признак создания через IoC
        /// </param>
        /// <returns>
        ///     Запланированное действие
        /// </returns>
        private QueuedAction AddToQueueInternal(ISyncAction action, bool autoCreated)
        {
            lock (this.lockObject)
            {
                var queuedAction = new QueuedAction(this.Container, action, autoCreated);
                this.ActionQueue.Enqueue(queuedAction);
                return queuedAction;
            }
        }

        /// <summary>
        ///     Внутренний метод добавления в очередь. Помимо прочего, требует указать признак
        ///     способа создания объекта. Объекты полученные из IoC контейнера будут освобождены
        ///     после исполнения
        /// </summary>
        /// <typeparam name="TParam">
        ///     Тип параметров
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Тип результата
        /// </typeparam>
        /// <param name="action">
        ///     Действие синхронизации
        /// </param>
        /// <param name="autoCreated">
        ///     Признак создания через IoC
        /// </param>
        /// <returns>
        ///     Запланированное действие
        /// </returns>
        private QueuedAction<TParam, TResult> AddToQueueInternal<TParam, TResult>(
            ISyncAction<TParam, TResult> action,
            bool autoCreated)
        {
            lock (this.lockObject)
            {
                var queuedAction = new QueuedAction<TParam, TResult>(this.Container, action, autoCreated);
                this.ActionQueue.Enqueue(queuedAction);
                return queuedAction;
            }
        }

        #endregion
    }
}