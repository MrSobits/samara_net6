namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Exceptions;
    using Gkh.Domain.ParameterVersioning.Proxy;
    using Gkh.Utils;
    using NHibernate;
    using NHibernate.Transform;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;

    using Dapper;

    /// <summary>
    /// Кэш изменений сущности
    /// </summary>
    public class EntityLogCache : IEntityLogCache
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IDomainService<PersonalAccountCalculatedParameter> parameterDomain;
        private bool initialized;
        private readonly Dictionary<long, bool> initializedMonths = new Dictionary<long, bool>();

        private Dictionary<long, EntityLogRecord[]> calcParameters;
        private Dictionary<string, EntityLogRecord[]> parameterCache;

        private readonly List<AccountLogRecord> calcParametersCacheList = new List<AccountLogRecord>();
        private readonly List<EntityLogRecord> parameterCacheList = new List<EntityLogRecord>();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public EntityLogCache(ISessionProvider sessionProvider, IDomainService<PersonalAccountCalculatedParameter> parameterDomain)
        {
            this.sessionProvider = sessionProvider;
            this.parameterDomain = parameterDomain;
        }

        /// <summary>
        /// Инициализировать
        /// </summary>
        /// <param name="accounts">Список счетов для которых создаётся кэш</param>
        public void Init(IQueryable<BasePersonalAccount> accounts)
        {
            if (this.initialized)
            {
                return;
            }

            var entityIds = this.GetEntityIds(accounts);

            this.Init(entityIds);
        }


        /// <summary>
        /// Получить массив идентификаторов из списка BasePersonalAccount
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        private long[] GetEntityIds(IQueryable<BasePersonalAccount> accounts)
        {
            var entityIds = accounts.Select(
                x => new
                {
                    Id = x.Id,
                    RoomId = x.Room.Id
                })
                .ToList()
                .SelectMany(x => new[] { x.Id, x.RoomId })
                .Distinct()
                .ToArray();
            return entityIds;
        }

        /// <summary>
        /// Инициализировать
        /// </summary>
        /// <param name="entityIds">Список идентификаторов создаётся кэш</param>
        public void Init(long[] entityIds)
        {
            if (this.initialized)
            {
                return;
            }

            var splitedEntityIds = entityIds.SplitArray().ToList();

            List<EntityLogRecord> logRecords;
            List<AccountLogRecord> calcParams;

            // попробуем паралельно получать
            if (splitedEntityIds.Count > 1)
            {
                logRecords = splitedEntityIds.AsParallel()
                .SelectMany(ids =>
                {
                    IStatelessSession session;

                    lock (this.sessionProvider)
                    {
                        session = this.sessionProvider.OpenStatelessSession();
                    }

                    using (session)
                    {
                        var conn = session.Connection;
                        var sql = "SELECT"
                            + "   entitylogl0_.ENTITY_ID AS \"EntityId\","
                            + "   entitylogl0_.PARAM_NAME AS \"ParameterName\","
                            + "   entitylogl0_.ID AS \"LogId\","
                            + "   entitylogl0_.DATE_ACTUAL AS \"DateActualChange\","
                            + "   entitylogl0_.CDATE_APPLIED AS \"DateApplied\","
                            + "   entitylogl0_.CPROP_VALUE AS \"PropertyValue\""
                            + " FROM"
                            + "   GKH_ENTITY_LOG_LIGHT entitylogl0_"
                            + " WHERE entitylogl0_.PARAM_NAME in ('room_area', 'area_share', 'account_open_date')"
                            + " and entitylogl0_.ENTITY_ID in (" + string.Join(",", ids) + ")";
                        return conn.Query<EntityLogRecord>(sql).ToList();
                    }
                })
                .ToList();

                calcParams = splitedEntityIds.AsParallel()
                    .SelectMany(ids =>
                    {
                        IStatelessSession session;

                        lock (this.sessionProvider)
                        {
                            session = this.sessionProvider.OpenStatelessSession();
                        }

                        using (session)
                        {
                            var repo = new Gkh.Domain.StatelessNhRepository<PersonalAccountCalculatedParameter>(session);
                            return repo.GetAll()
                                .Where(x => ids.Contains(x.PersonalAccount.Id))
                                .Select(x => new AccountLogRecord
                                {
                                    AccountId = x.PersonalAccount.Id,
                                    ParameterName = x.LoggedEntity.ParameterName,
                                    PropertyValue = x.LoggedEntity.PropertyValue,
                                    PropertyName = x.LoggedEntity.PropertyName,
                                    DateActualChange = x.LoggedEntity.DateActualChange,
                                    DateApplied = x.LoggedEntity.DateApplied,
                                    LogId = x.LoggedEntity.Id,
                                    EntityId = x.LoggedEntity.EntityId
                                })
                                .ToList();
                        }
                    })
                    .ToList();
            }
            else
            {
                using (var session = this.sessionProvider.OpenStatelessSession())
                {
                    var conn = session.Connection;
                    var sql = "SELECT"
                        + "   entitylogl0_.ENTITY_ID AS \"EntityId\","
                        + "   entitylogl0_.PARAM_NAME AS \"ParameterName\","
                        + "   entitylogl0_.ID AS \"LogId\","
                        + "   entitylogl0_.DATE_ACTUAL AS \"DateActualChange\","
                        + "   entitylogl0_.CDATE_APPLIED AS \"DateApplied\","
                        + "   entitylogl0_.CPROP_VALUE AS \"PropertyValue\""
                        + " FROM"
                        + "   GKH_ENTITY_LOG_LIGHT entitylogl0_"
                        + " WHERE entitylogl0_.PARAM_NAME in ('room_area', 'area_share', 'account_open_date')"
                        + " and entitylogl0_.ENTITY_ID in (" + string.Join(",", entityIds) + ")";
                    logRecords = conn.Query<EntityLogRecord>(sql).ToList();
                }

                calcParams = this.parameterDomain.GetAll()
                    .Where(x => entityIds.Contains(x.PersonalAccount.Id))
                    .Select(x => new AccountLogRecord
                    {
                        AccountId = x.PersonalAccount.Id,
                        ParameterName = x.LoggedEntity.ParameterName,
                        PropertyValue = x.LoggedEntity.PropertyValue,
                        PropertyName = x.LoggedEntity.PropertyName,
                        DateActualChange = x.LoggedEntity.DateActualChange,
                        DateApplied = x.LoggedEntity.DateApplied,
                        LogId = x.LoggedEntity.Id,
                        EntityId = x.LoggedEntity.EntityId
                    })
                    .ToList();
            }

            this.parameterCache = logRecords
                .GroupBy(x => "{0}|{1}".FormatUsing(x.EntityId, x.ParameterName))
                .ToDictionary(x => x.Key, x => x.ToArray());

            this.calcParameters = calcParams
                .GroupBy(x => x.AccountId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => new EntityLogRecord
                    {
                        ParameterName = y.ParameterName,
                        PropertyValue = y.PropertyValue,
                        DateActualChange = y.DateActualChange,
                        DateApplied = y.DateApplied,
                        LogId = y.LogId,
                        EntityId = y.EntityId
                    })
                    .ToArray());

            this.initialized = true;
        }

        /// <summary>
        /// Получить данные за указанный месяц
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="period"></param>
        public void GetEntityLogsByMonth(IQueryable<BasePersonalAccount> accounts, ChargePeriod period)
        {
            if (this.initializedMonths.ContainsKey(period.Id))
            {
                return;
            }

            var entityIds = this.GetEntityIds(accounts);
            this.GetEntityLogsByMonth(entityIds, period);

            this.initializedMonths[period.Id] = true;
        }

        /// <summary>
        /// Получить данные за указанный месяц
        /// </summary>
        /// <param name="entityIds">Список идентификаторов создаётся кэш</param>
        /// <param name="period"></param>
        private void GetEntityLogsByMonth(long[] entityIds, ChargePeriod period)
        {
            List<EntityLogRecord> logRecords;

            using (var session = this.sessionProvider.OpenStatelessSession())
            {
                var conn = session.Connection;
                var sql = "SELECT"
                    + "   entitylogl0_.ENTITY_ID AS \"EntityId\","
                    + "   entitylogl0_.PARAM_NAME AS \"ParameterName\","
                    + "   entitylogl0_.ID AS \"LogId\","
                    + "   entitylogl0_.DATE_ACTUAL AS \"DateActualChange\","
                    + "   entitylogl0_.CDATE_APPLIED AS \"DateApplied\","
                    + "   entitylogl0_.CPROP_VALUE AS \"PropertyValue\""
                    + " FROM"
                    + "   GKH_ENTITY_LOG_LIGHT entitylogl0_"
                    + " WHERE"
                    + " entitylogl0_.CDATE_APPLIED >= " + period.StartDate.ToShortDateStringWithQuote() + "::DATE "
                    + " and entitylogl0_.PARAM_NAME in ('room_area', 'area_share', 'account_open_date')"
                    + " and entitylogl0_.ENTITY_ID in (" + string.Join(",", entityIds) + ")";
                logRecords = conn.Query<EntityLogRecord>(sql).ToList();
            }

            var calcParams = this.parameterDomain.GetAll()
                .Where(x => entityIds.Contains(x.PersonalAccount.Id))
                .Select(x => new AccountLogRecord
                {
                    AccountId = x.PersonalAccount.Id,
                    ParameterName = x.LoggedEntity.ParameterName,
                    PropertyValue = x.LoggedEntity.PropertyValue,
                    PropertyName = x.LoggedEntity.PropertyName,
                    DateActualChange = x.LoggedEntity.DateActualChange,
                    DateApplied = x.LoggedEntity.DateApplied,
                    LogId = x.LoggedEntity.Id,
                    EntityId = x.LoggedEntity.EntityId
                })
                .ToList();


            this.parameterCacheList.AddRange(logRecords);
            this.calcParametersCacheList.AddRange(calcParams);

        }

        /// <summary>
        /// Включить полученнные данные по месяцам в кэш расчета
        /// </summary>
        public void IncludeInCacheEntityLogsByMonth()
        {
            if (this.initialized)
            {
                return;
            }

            this.parameterCache = this.parameterCacheList
              .GroupBy(x => "{0}|{1}".FormatUsing(x.EntityId, x.ParameterName))
              .ToDictionary(x => x.Key, x => x.ToArray());


            this.calcParameters = this.calcParametersCacheList
                .GroupBy(x => x.AccountId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => new EntityLogRecord
                    {
                        ParameterName = y.ParameterName,
                        PropertyValue = y.PropertyValue,
                        DateActualChange = y.DateActualChange,
                        DateApplied = y.DateApplied,
                        LogId = y.LogId,
                        EntityId = y.EntityId
                    })
                    .ToArray());

            this.initialized = true;
        }


        /// <summary>
        /// Получить кеш по ключу
        /// </summary>
        /// <returns>Список версий параметров</returns>
        public IEnumerable<EntityLogRecord> GetAllParameters(string key)
        {
            this.ThrowIfNotInitialized();

            return this.parameterCache.Get(key) ?? new EntityLogRecord[0];
        }

        /// <summary>
        /// Получить кэш по счёту
        /// </summary>
        /// <param name="account">счёт</param>
        /// <returns>Коллекция</returns>
        public IEnumerable<EntityLogRecord> GetCalculatedParams(BasePersonalAccount account)
        {
            this.ThrowIfNotInitialized();

            return this.calcParameters.Get(account.Id) ?? new EntityLogRecord[0];
        }

        /// <summary>
        /// Определить максимальную глубину перерасчета вызванного изменением версионируемых параметров
        /// </summary>
        /// <param name="accountsIds">Список идентификаторов счетов</param>
        /// <param name="period">Расчетный период для которого определяем глубину перерасчета</param>
        /// <returns></returns>
        public Dictionary<long, DateTime> GetStartDateRecalc(IQueryable<BasePersonalAccount> accountsQueryable, ChargePeriod period)
        {
            List<EntityLogRecord> logRecords;
            var accounts = accountsQueryable.ToList();
            var accountsIds = accounts.Select(x => x.Id).ToArray();
            var listAccountParams = new List<string> { "account_open_date", "area_share" };
            lock (this.sessionProvider)
            {
                using (var session = this.sessionProvider.OpenStatelessSession())
                {
                    var entitiesIds = this.GetEntityIds(accountsQueryable);
                    var conn = session.Connection;
                    var sql = "SELECT"
                        + "   entitylogl0_.ENTITY_ID AS \"EntityId\","
                        + "   entitylogl0_.PARAM_NAME AS \"ParameterName\","
                        + "   entitylogl0_.ID AS \"LogId\","
                        + "   entitylogl0_.DATE_ACTUAL AS \"DateActualChange\","
                        + "   entitylogl0_.CDATE_APPLIED AS \"DateApplied\","
                        + "   entitylogl0_.CPROP_VALUE AS \"PropertyValue\""
                        + " FROM"
                        + "   GKH_ENTITY_LOG_LIGHT entitylogl0_"
                        + " WHERE"
                        + " entitylogl0_.PARAM_NAME in ('room_area', 'area_share', 'account_open_date')"
                        + " and entitylogl0_.ENTITY_ID in (" + string.Join(",", entitiesIds) + ")";
                    logRecords = conn.Query<EntityLogRecord>(sql).ToList();

                    //id помещений
                    var roomIds = accounts.Select(x => x.Room.Id).ToArray();
                    //удалили лишние записи по помещениям
                    logRecords.RemoveAll(x => x.ParameterName == "room_area" && !roomIds.Contains(x.EntityId));
                    //удалили лишние записи по ЛС
                    logRecords.RemoveAll(x => listAccountParams.Contains(x.ParameterName) && !accountsIds.Contains(x.EntityId));
                }
            }

            var calcParams = this.parameterDomain.GetAll()
                .Where(x => accountsIds.Contains(x.PersonalAccount.Id))
                .Select(x => new AccountLogRecord
                {
                    AccountId = x.PersonalAccount.Id,
                    ParameterName = x.LoggedEntity.ParameterName,
                    PropertyValue = x.LoggedEntity.PropertyValue,
                    PropertyName = x.LoggedEntity.PropertyName,
                    DateActualChange = x.LoggedEntity.DateActualChange,
                    DateApplied = x.LoggedEntity.DateApplied,
                    LogId = x.LoggedEntity.Id,
                    EntityId = x.LoggedEntity.EntityId
                })
                .ToList();

            var logRecordsWithAccountId = logRecords.Select(x => new EntityLogRecordUsed(x) {AccountId = x.EntityId}).ToList();
            var listRoomEntities = new List<EntityLogRecordUsed>();
            //размножаем записи по помещениям
            foreach (var roomRecord in logRecordsWithAccountId.Where(x => !listAccountParams.Contains(x.ParameterName)))
            {
                listRoomEntities.AddRange(
                    accounts.Where(x => x.Room.Id == roomRecord.EntityId).Select(x => new EntityLogRecordUsed(roomRecord) {AccountId = x.Id}));
            }

            logRecordsWithAccountId = logRecordsWithAccountId.Where(x => listAccountParams.Contains(x.ParameterName)).Union(listRoomEntities).ToList();
            //получаем список не учтенных в перерасчете изменений параметров
            var notRecalculatedLogRecords =
                logRecordsWithAccountId.Where(x => !calcParams.Any(c => c.LogId == x.LogId && c.AccountId == x.AccountId)).ToList();
          
            //получаем максимальную глубину перерасчета на основе изменений параметров
            return notRecalculatedLogRecords.GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, records => records.SafeMin(x => x.DateActualChange, DateTime.MaxValue));
        }

        private void ThrowIfNotInitialized()
        {
            if (!this.initialized)
            {
                throw new CacheNotInitializedException("Entity log cache");
            }
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            if (this.initialized)
            {
                this.calcParameters.Clear();
                this.parameterCache.Clear();

                this.initialized = false;
            }
        }

        private class AccountLogRecord
        {
            public long AccountId { get; set; }

            public string ParameterName { get; set; }

            public string PropertyValue { get; set; }

            public string PropertyName { get; set; }

            public DateTime DateActualChange { get; set; }

            public DateTime? DateApplied { get; set; }

            public long LogId { get; set; }

            public long EntityId { get; set; }
        }
    }
}