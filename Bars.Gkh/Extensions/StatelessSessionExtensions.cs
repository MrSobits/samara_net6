namespace Bars.Gkh.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;

    using Castle.Windsor;

    using NHibernate;

    /// <summary>
    /// Методы-расширения для <see cref="IStatelessSession"/>
    /// </summary>
    public static class StatelessSessionExtensions
    {
        /// <summary>
        /// Помошник для работы с прокси классами ORM (singleton)
        /// </summary>
        private static readonly IUnProxy UnProxy = ApplicationContext.Current.Container.Resolve<IUnProxy>();

        /// <summary>
        /// Выполнить действие в транзакции <see cref="IStatelessSession"/>
        /// </summary>
        /// <param name="sessionProvider">Поставщик сессии</param>
        /// <param name="action">Действие</param>
        public static void InStatelessTransaction(this ISessionProvider sessionProvider, Action<IStatelessSession> action)
        {
            using (var session = sessionProvider.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    action(session);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Выполнить действие в транзакции <see cref="IDbConnection"/> в <see cref="IStatelessSession"/>
        /// <para>Коммит транзакции происходит после выполнения <paramref name="action"/>'а</para>
        /// </summary>
        /// <param name="sessionProvider">Поставщик сессии</param>
        /// <param name="action">Действие</param>
        public static void InStatelessConnectionTransaction(this ISessionProvider sessionProvider, Action<IDbConnection, IDbTransaction> action)
        {
            using (var session = sessionProvider.OpenStatelessSession())
            {
                var connection = session.Connection;
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        action(connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Выполнить действие в транзакции <see cref="IStatelessSession"/>
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="action">Действие</param>
        public static void InStatelessTransaction(this IWindsorContainer container, Action<IStatelessSession> action)
        {
            container.Resolve<ISessionProvider>().InStatelessTransaction(action);
        }
        /// <summary>
        /// Выполнить действие в транзакции <see cref="IDbConnection"/> в <see cref="IStatelessSession"/>
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="action">Действие</param>
        public static void InStatelessConnectionTransaction(this IWindsorContainer container, Action<IDbConnection, IDbTransaction> action)
        {
            container.Resolve<ISessionProvider>().InStatelessConnectionTransaction(action);
        }

        /// <summary>
        /// Выполнить вставку или обновление сущности с обновлением всех сервисных полей
        /// </summary>
        /// <param name="statelessSession">Сессия</param>
        /// <param name="entity">Сущность</param>
        public static void InsertOrUpdate(this IStatelessSession statelessSession, object entity)
        {
            var persistentObject = StatelessSessionExtensions.UnProxy.GetUnProxyObject(entity) as PersistentObject;
            if (persistentObject == null)
            {
                return;
            }

            var baseEntity = persistentObject as BaseEntity;
            if (baseEntity != null)
            {
                baseEntity.ObjectEditDate = DateTime.Now;
            }

            if (persistentObject.Id == 0)
            {
                if (baseEntity != null)
                {
                    baseEntity.ObjectCreateDate = DateTime.Now;
                }

                statelessSession.Insert(persistentObject);
            }
            else
            {
                if (baseEntity != null)
                {
                    baseEntity.ObjectVersion += 1;
                }

                statelessSession.Update(persistentObject);
            }
        }

        /// <summary>
        /// Выполнить <see cref="saveAction"/> в транзакции для каждого экземпляра <see cref="actionParams"/>
        /// <para>
        /// <see cref="saveAction"/> должен вызывать для объектов <see cref="IStatelessSession.Insert(object)"/> / <see cref="IStatelessSession.Delete(object)"/> / <see cref="IStatelessSession.Update(object)"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T">Тип параметра для <see cref="saveAction"/></typeparam>
        /// <param name="statelessSession">Сессия</param>
        /// <param name="actionParams">Коллекция параметров</param>
        /// <param name="saveAction">Делегат сохранения для каждого объекта <see cref="actionParams"/></param>
        public static void SaveInTransaction<T>(this IStatelessSession statelessSession, IEnumerable<T> actionParams, Action<IStatelessSession, T> saveAction)
            where T : class
        {
            using (var transaction = statelessSession.BeginTransaction())
            {
                try
                {
                    foreach (var actionParam in actionParams)
                    {
                        saveAction(statelessSession, actionParam);
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}