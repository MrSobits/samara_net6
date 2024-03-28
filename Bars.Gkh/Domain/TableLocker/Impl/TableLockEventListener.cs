namespace Bars.Gkh.Domain.TableLocker.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using B4.DataAccess;

    using Castle.MicroKernel.Lifestyle;

    using Castle.Windsor;
    using NHibernate.Action;
    using NHibernate.Cfg;
    using NHibernate.Engine;
    using NHibernate.Event;
    using NHibernate.Event.Default;
    using NHibernate.Exceptions;

    using Environment = NHibernate.Cfg.Environment;

    /// <summary>
    /// Listener для таблицы блокировки
    /// </summary>
    public class TableLockFlushEventListener : AbstractFlushingEventListener,
                                               IFlushEventListener,
                                               IAutoFlushEventListener
    {
        [ThreadStatic]
        private static IBatchTableLocker locker;

        private static readonly object Lock = new object();

        private readonly IWindsorContainer _container;

        private readonly FieldInfo _deletions;

        private readonly FieldInfo _insertions;

        private readonly FieldInfo _updates;

        /// <summary>
        /// Лист событий таблицы блокировок 
        /// </summary>
        /// <param name="container">Контейнер</param>
        public TableLockFlushEventListener(IWindsorContainer container)
        {
            _container = container;

            var t = typeof(ActionQueue);
            _insertions = t.GetField("insertions", BindingFlags.NonPublic | BindingFlags.Instance);
            _deletions = t.GetField("deletions", BindingFlags.NonPublic | BindingFlags.Instance);
            _updates = t.GetField("updates", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <inheritdoc />
        public async Task OnAutoFlushAsync(AutoFlushEvent @event, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                using var scope = this._container.BeginScope();
                this.CheckLocked(@event);
            }, cancellationToken);
        }

        /// <summary>
        /// Обработчик  AutoFlush
        /// </summary>
        /// <param name="event">Событие</param>
        public void OnAutoFlush(AutoFlushEvent @event)
            => CheckLocked(@event);

        /// <inheritdoc />
        public async Task OnFlushAsync(FlushEvent @event, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                using (this._container.BeginScope())
                {
                    CheckLocked(@event);
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Обработчик  Flush
        /// </summary>
        /// <param name="event">Событие</param>
        public void OnFlush(FlushEvent @event)
            => CheckLocked(@event);

        private void CheckLocked(FlushEvent @event)
        {
            var oldSize = @event.Session.ActionQueue.CollectionRemovalsCount;

            try
            {
                FlushEverythingToExecutions(@event);

                var actions = ExtractActions(@event.Session.ActionQueue);
                var entities = actions.GroupBy(x => x.Key.Instance.GetType())
                                      .ToDictionary(x => x.Key, x => x.Select(y => y.Value).Distinct());
                if (entities.Count == 0)
                {
                    return;
                }

                if (locker == null)
                {
                    lock (Lock)
                    {
                        if (locker == null)
                        {
                            locker = _container.Resolve<IBatchTableLocker>();
                        }
                    }
                }

                lock (locker)
                {
                    foreach (var entity in entities)
                    {
                        foreach (var action in entity.Value)
                        {
                            locker.With(entity.Key, action);
                        }
                    }

                    if (locker.CheckLocked())
                    {
                        locker.Clear();
                        throw new TableLockedException();
                    }

                    locker.Clear();
                }
            }
            finally
            {
                @event.Session.ActionQueue.ClearFromFlushNeededCheck(oldSize);
                this._container.Release(TableLockFlushEventListener.locker);
                TableLockFlushEventListener.locker = null;
            }
        }

        private IEnumerable<KeyValuePair<EntityAction, string>> ExtractActions(ActionQueue queue)
        {
            foreach (var action in ((List<AbstractEntityInsertAction>)_insertions.GetValue(queue)))
            {
                yield return new KeyValuePair<EntityAction, string>(action, "INSERT");
            }

            foreach (var action in (List<EntityDeleteAction>)_deletions.GetValue(queue))
            {
                yield return new KeyValuePair<EntityAction, string>(action, "DELETE");
            }

            foreach (var action in (List<EntityUpdateAction>)_updates.GetValue(queue))
            {
                yield return new KeyValuePair<EntityAction, string>(action, "UPDATE");
            }
        }
    }

    /// <summary>
    /// Конфигурационная модель таблицы блокировки
    /// </summary>
    public class TableLockNhConfigModifier : INhibernateConfigModifier
    {
        /// <summary>
        /// Контейнер 
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <summary>
        /// Применять конфигурация 
        /// </summary>
        /// <param name="configuration">Конфигурации</param>
        public void Apply(Configuration configuration)
        {
            var listener = new TableLockFlushEventListener(Container);

            configuration.SetProperty(
                Environment.SqlExceptionConverter,
                typeof(TableLockExceptionConverter).AssemblyQualifiedName);

            configuration.EventListeners.AutoFlushEventListeners = Concat(
                listener,
                configuration.EventListeners.AutoFlushEventListeners);
            configuration.EventListeners.FlushEventListeners = Concat(
                listener,
                configuration.EventListeners.FlushEventListeners);
        }

        private static T[] Concat<T>(T v1, IEnumerable<T> coll)
        {
            var resultList = new List<T> { v1 };
            resultList.AddRange(coll);
            return resultList.ToArray();
        }
    }

    /// <summary>
    /// Конвертер ошибок таблицы блокировок
    /// </summary>
    public class TableLockExceptionConverter : ISQLExceptionConverter
    {

        /// <summary>
        /// Конвертер ошибок таблицы блокировок
        /// </summary>
        /// <param name="extracter">Ошибка</param>
        public TableLockExceptionConverter(IViolatedConstraintNameExtracter extracter)
        {
        }

        /// <summary>
        /// Конвертер 
        /// </summary>
        /// <param name="exceptionInfo"> Информаци об ошибке </param>
        /// <returns></returns>
        public Exception Convert(AdoExceptionContextInfo exceptionInfo)
        {
            if (exceptionInfo.SqlException.Message.Contains("TABLE_LOCKED_EXCEPTION"))
            {
                if (string.IsNullOrEmpty(exceptionInfo.EntityName))
                {
                    throw new TableLockedException();
                }

                throw new TableLockedException();
            }

            return SQLStateConverter.HandledNonSpecificException(
                exceptionInfo.SqlException,
                exceptionInfo.Message,
                exceptionInfo.Sql);
        }
    }
}