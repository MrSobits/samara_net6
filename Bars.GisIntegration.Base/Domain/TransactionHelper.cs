namespace Bars.GisIntegration.Base.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Castle.Windsor;

    using NLog;

    public class TransactionHelper
    {
        private static Logger log = LogManager.GetLogger(typeof(TransactionHelper).FullName);

        /// <summary>
        ///     Сохранение сущностей с разделением на несколько транзакций
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="container">
        ///     <see cref="IWindsorContainer">Контейнер</see>
        /// </param>
        /// <param name="items">Сущности на сохранение</param>
        /// <param name="transactionSize">Количество сохранений в одной транзации</param>
        /// <param name="clearAfterDone">Очистить список сущностей после сохранения</param>
        /// <param name="useStatelessSession">Использовать stateless сессию</param>
        public static void InsertInManyTransactions<T>(
            IWindsorContainer container,
            IEnumerable<T> items,
            int transactionSize = 1000,
            bool clearAfterDone = true,
            bool useStatelessSession = false) where T : PersistentObject
        {
            if (useStatelessSession)
            {
                TransactionHelper.InsertWithStatelessSession(container, items, transactionSize);
            }
            else
            {
                TransactionHelper.InsertWithNhibernate(container, items, transactionSize);
            }

            if (clearAfterDone)
            {
                items = null;
            }
        }

        private static void InsertWithNhibernate<T>(
            IWindsorContainer container,
            IEnumerable<T> items,
            int transactionSize) where T : PersistentObject
        {
            var totalCount = items.Count();

            if (totalCount == 0)
            {
                return;
            }

            var domain = container.ResolveDomain<T>();
            using (container.Using(domain))
            {
                var retryCount = 3;
                var savedCount = 0;
                while (savedCount <= totalCount)
                {
                    var toSave = items.Skip(savedCount).Take(transactionSize).ToArray();
                    var retryCounter = 0;

                    while (retryCounter < retryCount)
                    {
                        try
                        {
                            using (var tr = container.Resolve<IDataTransaction>())
                            {
                                try
                                {
                                    foreach (var item in toSave)
                                    {
                                        if (item.Id > 0)
                                        {
                                            domain.Update(item);
                                        }
                                        else
                                        {
                                            domain.Save(item);
                                        }
                                    }

                                    tr.Commit();

                                    savedCount += transactionSize;
                                    toSave = null;

                                    break;
                                }
                                catch
                                {
                                    tr.Rollback();
                                    throw;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (e is TableLockedException)
                            {
                                throw;
                            }

                            TransactionHelper.log.Error(e);

                            retryCounter++;

                            if (retryCounter == retryCount)
                            {
                                throw new Exception(
                                    "Ошибка сохранения записей после {0} попыток".FormatUsing(retryCount),
                                    e);
                            }
                        }
                    }
                }
            }
        }

        private static void InsertWithStatelessSession<T>(
            IWindsorContainer container,
            IEnumerable<T> items,
            int transactionSize) where T : PersistentObject
        {
            var totalCount = items.Count();

            if (totalCount == 0)
            {
                return;
            }

            var unProxy = container.Resolve<IUnProxy>();

            var sessionProvider = container.Resolve<ISessionProvider>();

            using (container.Using(sessionProvider))
            {
                var savedCount = 0;
                var retryCount = 3;
                var batchingSupported = true;
                while (savedCount <= totalCount)
                {
                    using (var session = sessionProvider.OpenStatelessSession())
                    {
                        if (batchingSupported)
                        {
                            try
                            {
                                // оракл + стэйтлесс = отключенный батчер
                                // отключенный батчер + SetBatchSize = NotSupportedException
                                session.SetBatchSize(transactionSize);
                            }
                            catch (NotSupportedException)
                            {
                                batchingSupported = false;
                            }
                        }

                        var toSave = items.Skip(savedCount).Take(transactionSize).ToArray();
                        var retryCounter = 0;

                        while (retryCounter < retryCount)
                        {
                            try
                            {
                                using (var tr = session.BeginTransaction(IsolationLevel.ReadCommitted))
                                {
                                    try
                                    {
                                        foreach (var item in toSave)
                                        {
                                            var persistentObject = unProxy.GetUnProxyObject(item) as PersistentObject;

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

                                                session.Insert(persistentObject);
                                            }
                                            else
                                            {
                                                if (baseEntity != null)
                                                {
                                                    baseEntity.ObjectVersion += 1;
                                                }

                                                session.Update(persistentObject);
                                            }
                                        }

                                        tr.Commit();

                                        savedCount += transactionSize;
                                        toSave = null;

                                        break;
                                    }
                                    catch
                                    {
                                        tr.Rollback();
                                        throw;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                if (e.ToString().Contains("TABLE_LOCKED_EXCEPTION"))
                                {
                                    throw new TableLockedException();
                                }

                                TransactionHelper.log.Error(e);

                                retryCounter++;

                                if (retryCounter == retryCount)
                                {
                                    throw new Exception(
                                        "Ошибка сохранения записей в Stateless сессии после {0} попыток".FormatUsing(
                                            retryCount),
                                        e);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}