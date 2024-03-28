namespace Bars.Gkh.Domain.TableLocker.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    using NHibernate.Linq;

    /// <summary>
    ///     Сервис работы с блокировками таблиц
    /// </summary>
    public class TableLockService : ITableLockService
    {
        private readonly IWindsorContainer container;

        /// <summary>
        ///     .ctor
        /// </summary>
        /// <param name="container"></param>
        public TableLockService(IWindsorContainer container)
        {
            this.container = container;
        }

        public IDataResult List(BaseParams baseParams)
        {
            var provider = container.Resolve<ISessionProvider>();
            try
            {
                using (var session = provider.GetCurrentSession())
                {
                    var loadParams = baseParams.GetLoadParam();
                    var query =
                        session.Query<TableLock>()
                               .Select(x => new { x.TableName, x.LockStart, x.Action })
                               .Filter(loadParams, container);
                    var total = query.Count();
                    return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToArray(), total);
                }
            }
            finally
            {
                container.Release(provider);
            }
        }

        public void Unlock(BaseParams baseParams)
        {
            var service = container.Resolve<ITableLocker>();
            try
            {
                service.Unlock(
                    baseParams.Params.Get("tableName", string.Empty),
                    baseParams.Params.Get("action", string.Empty));
            }
            finally
            {
                container.Release(service);
            }
        }

        public void UnlockAll()
        {
            var provider = container.Resolve<ISessionProvider>();
            var service = container.Resolve<IBatchTableLocker>();
            try
            {
                using (var session = provider.GetCurrentSession())
                {
                    var query = session.Query<TableLock>().Select(x => new { x.TableName, x.Action });
                    foreach (var rec in query)
                    {
                        service.With(rec.TableName, rec.Action);
                    }

                    service.Unlock();
                }
            }
            finally
            {
                container.Release(provider);
                container.Release(service);
            }
        }
    }
}