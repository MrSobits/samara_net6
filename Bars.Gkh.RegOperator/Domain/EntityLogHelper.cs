namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using B4.IoC;
    using Gkh.Domain.ParameterVersioning.Proxy;

    using Castle.Windsor;
    using Entities;
    using Gkh.Entities;
    using ParametersVersioning;

    /// <summary>
    /// хелпер логов сущности
    /// </summary>
    public class EntityLogHelper
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container"></param>
        public EntityLogHelper(IWindsorContainer container)
        {
            this.logRepo = container.ResolveDomain<EntityLogLight>();
            this.globalCache = container.Resolve<IEntityLogCache>();
            this.container = container;
        }

        /// <summary>
        /// Получить текущее актуальное значение
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityLogLight GetCurrentAppliedValue(string parameterName, PersistentObject entity)
        {
            var id = entity.Return(a => a.Id);

            var propValue = this.logRepo.GetAll()
                    .Where(x => x.EntityId == id
                                && x.ParameterName == parameterName
                                && !x.DateEnd.HasValue
                                && x.DateApplied.HasValue)
                    .OrderByDescending(x => x.DateApplied)
                    .FirstOrDefault();

            return propValue;
        }

        /// <summary>
        /// Получение последнего расчитанного значения по дате, либо последнего установленного значения
        /// </summary>
        /// <param name="parameterName">Имя версионируемого параметра</param>
        /// <param name="entity">Сущность, чье свойство версионируется</param>
        /// <param name="date">Дата, от которой выьирать версию</param>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Сущность версии</returns>
        public EntityLogRecord GetLastCalculatedOrLastAppliedByDate(
            string parameterName,
            PersistentObject entity,
            DateTime date,
            BasePersonalAccount account)
        {
            var baseQuery = this.GetLogFromCache(entity, parameterName).OrderByDescending(x => x.DateApplied);

            var lastCalculated = this.GetUsedInRecalculation(account)
                    .Where(x => x.ParameterName == parameterName)
                    .OrderByDescending(x => x.DateApplied)
                    .FirstOrDefault(x => x.DateApplied <= date)
                    .Return(x => x);

            if (lastCalculated == null)
            {
                var lastApplied = baseQuery.FirstOrDefault(x => x.DateApplied <= date);
                // Tip. Если дата открытия меньше чем дата первого расчета, то запрос выше вернет null
                // поэтому в таком случае будм возвращать просто последнее установленное значение
                return lastApplied
                       ?? baseQuery.OrderByDescending(x => x.DateApplied)
                           .FirstOrDefault(x => x.DateActualChange.Date <= date);
            }

            return lastCalculated;
        }

        /// <summary>
        /// Получаем параметры, изменившиеся в период <see cref="period"/>, но по которым не было перерасчета
        /// </summary>
        /// <param name="parameterName">Имя параметра</param>
        /// <param name="entity">Сущность с изменененным параметром</param>
        /// <param name="period">Период начисления</param>
        /// <param name="account">ЛС</param>
        public IEnumerable<EntityLogRecord> GetUncalcAppliedInPeriod(
            string parameterName,
            PersistentObject entity,
            IPeriod period,
            BasePersonalAccount account)
        {
            var history = this.GetLogFromCache(entity, parameterName)
                    .Where(x => x.DateApplied >= period.StartDate)
                    .Where(x => !period.EndDate.HasValue || period.EndDate >= x.DateApplied)
                    .Where(x => this.GetUsedInRecalculation(account).All(c => c.LogId != x.LogId));

            return history;
        }

        public IEnumerable<EntityLogRecord> GetUncalcAppliedInRange(
            string parameterName,
            PersistentObject entity,
            DateTime date,
            BasePersonalAccount account)
        {
            var history = this.GetLogFromCache(entity, parameterName)
                    .Where(x => x.DateActualChange <= date)
                    .Where(x => this.GetUsedInRecalculation(account).All(c => c.LogId != x.LogId));

            return history;
        }

        /// <summary>
        /// Получить актуальные значения за период
        /// </summary>
        /// <param name="parameterName">Имя параметра</param>
        /// <param name="entities">Версионируемые сущности</param>
        /// <param name="to"></param>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список актуальных значений</returns>
        public IEnumerable<EntityLogRecord> GetActualChangesInRange(
            string parameterName,
            IEnumerable<PersistentObject> entities,
            DateTime to,
            BasePersonalAccount account)
        {
            to = to.Date.AddDays(1).AddMilliseconds(-1);

            var allChanges = entities.SelectMany(x => this.GetLogFromCache(x, parameterName));

            var history = allChanges.Where(x => x.DateActualChange.Date <= to);

            return history;
        }

        /// <summary>
        /// Очистить таблицу оставляя только последний параметр по дате поступления
        /// </summary>
        public void ClearTablebutLeaveLastApplied()
        {
            var logs = this.logRepo.GetAll()
                    .Select(x => new
                    {
                        x.ParameterName,
                        x.EntityId,
                        x.Id,
                        x.DateApplied
                    })
                    .ToList()
                    .GroupBy(x => new { x.ParameterName, x.EntityId })
                    .SelectMany(x => x.OrderByDescending(y => y.DateApplied).Skip(1)).ToList();

            var sessions = this.container.Resolve<ISessionProvider>();

            using (this.container.Using(sessions))
            {
                var session = sessions.OpenStatelessSession();

                for (var i = 0; i < logs.Count; i += 1000)
                {
                    var ids = logs.Select(x => x.Id).Skip(i).Take(1000).ToList();

                    session.CreateQuery(" delete from EntityLogLight" +
                                        " where Id in (:ids)")
                        .SetParameterList("ids", ids)
                        .ExecuteUpdate();
                }
            }
        }

        private IEnumerable<EntityLogRecord> GetLogFromCache(PersistentObject entity, string parameterName)
        {
            return this.globalCache.GetAllParameters(string.Format("{0}|{1}", entity.Id, parameterName));
        }

        private IEnumerable<EntityLogRecord> GetUsedInRecalculation(BasePersonalAccount account)
        {
            return this.globalCache.GetCalculatedParams(account);
        }

        private readonly IDomainService<EntityLogLight> logRepo;
        private readonly IEntityLogCache globalCache;
        private readonly IWindsorContainer container;
    }
}