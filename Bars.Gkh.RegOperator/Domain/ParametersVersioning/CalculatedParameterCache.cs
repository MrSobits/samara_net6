namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using B4.Utils;
    using B4.Utils.Annotations;
    using B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;
    using Newtonsoft.Json;
    using NHibernate;
    using NHibernate.Transform;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Кэш параметров
    /// </summary>
    public class CalculatedParameterCache : ICalculatedParameterCache
    {
        private bool initialized;
        private Dictionary<long, Dictionary<long, Dictionary<string, object>>> cache;
        private readonly ISessionProvider sessionProvider;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="sessionProvider">Провайдер сессий</param>
        public CalculatedParameterCache(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="periods">Периоды</param>
        public void Init(IQueryable<BasePersonalAccount> query, IPeriod[] periods)
        {
            ArgumentChecker.NotNull(query, "query");
            ArgumentChecker.NotNull(periods, "periods");

            if (!this.initialized)
            {
                var entityIds = query.Select(x => x.Id).ToArray();
                this.Init(entityIds, periods);
            }
        }

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="accountIds">Список идентификаторов</param>
        /// <param name="periods">Периоды</param>
        public void Init(long[] accountIds, IPeriod[] periods)
        {
            ArgumentChecker.NotNull(accountIds, "accountIds");
            ArgumentChecker.NotNull(periods, "periods");

            if (this.initialized)
            {
                return;
            }
            
            List<CalcParamDto> parametersList;
            var entityIdsSplitted = accountIds.SplitArray().ToList();
            var periodIds = periods.Select(x => x.Id).ToArray();

            if (entityIdsSplitted.Count > 1)
            {
                parametersList = entityIdsSplitted.AsParallel()
                    .SelectMany(ids =>
                    {
                        IStatelessSession session;

                        lock (this.sessionProvider)
                        {
                            session = this.sessionProvider.OpenStatelessSession();
                        }

                        using (session)
                        {
                            return session.CreateSQLQuery(@"
                                SELECT ca.pers_acc_id as ""AccountId"", 
                                    ca.period_id as ""PeriodId"", 
                                    pt.param_values as ""StringParameters""
                                FROM regop_pers_acc_charge ca
                                    INNER JOIN regop_calc_param_trace pt ON pt.calc_guid = ca.guid
                                WHERE ca.pers_acc_id IN (:ids)
                                    AND ca.period_id IN (:periodIds)
                                    AND pt.period_id IN (:periodIds)
                                    AND pt.calc_type = 0")
                                .SetParameterList("ids", ids)
                                .SetParameterList("periodIds", periodIds)
                                .SetResultTransformer(Transformers.AliasToBean<CalcParamDto>())
                                .List<CalcParamDto>()
                                .ToList();
                        }
                    })
                    .ToList();
            }
            else
            {
                using (var session = this.sessionProvider.OpenStatelessSession())
                {
                    parametersList = session.CreateSQLQuery(@"
                             SELECT ca.pers_acc_id as ""AccountId"", 
                                ca.period_id as ""PeriodId"",
                                pt.param_values as ""StringParameters""
                             FROM regop_pers_acc_charge ca
                                 INNER JOIN regop_calc_param_trace pt ON pt.calc_guid = ca.guid
                             WHERE ca.pers_acc_id IN (:ids)
                                 AND ca.period_id IN (:periodIds)
                                 AND pt.period_id IN (:periodIds)
                                 AND pt.calc_type = 0")
                        .SetParameterList("ids", accountIds)
                        .SetParameterList("periodIds", periodIds)
                        .SetResultTransformer(Transformers.AliasToBean<CalcParamDto>())
                        .List<CalcParamDto>()
                        .ToList();
                }
            }

            this.cache = parametersList
                .GroupBy(x => x.PeriodId)
                .ToDictionary(x => x.Key, x => x
                    .GroupBy(y => y.AccountId)
                    .ToDictionary(y => y.Key, y => y.First().Parameters));

            this.initialized = true;
        }

        /// <summary>
        /// Получить кэш по счёту
        /// </summary>
        /// <param name="account">Счёт</param>
        /// <param name="period">Период</param>
        /// <returns>Кэш</returns>
        public Dictionary<string, object> GetParameters(BasePersonalAccount account, IPeriod period)
        {
            return this.cache.Get(period.Id)?.Get(account.Id);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.initialized)
            {
                this.cache.Clear();
            }
        }

        private class CalcParamDto
        {
            public long AccountId { get; set; }

            public long PeriodId { get; set; }

            //не делать приватным
            public string StringParameters { get; set; }

            public Dictionary<string, object> Parameters
            {
                get
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, object>>(this.StringParameters);
                }
            }
        }
    }
}