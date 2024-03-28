namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;

    using Entities;
    using Gkh.Domain.ParameterVersioning.Proxy;

    /// <summary>
    /// Кеш лога изменения сущностей
    /// </summary>
    public interface IEntityLogCache : IDisposable
    {
        /// <summary>
        /// Инициализировать
        /// </summary>
        /// <param name="accounts">Список счетов для которых создаётся кэш</param>
        void Init(IQueryable<BasePersonalAccount> accounts);

        /// <summary>
        /// Инициализировать
        /// </summary>
        /// <param name="entityIds">Список идентификаторов создаётся кэш</param>
        void Init(long[] entityIds);

        /// <summary>
        /// Получить кеш
        /// </summary>
        IEnumerable<EntityLogRecord> GetAllParameters(string key);

        /// <summary>
        /// Получение параметров, расчитанных для ЛС
        /// </summary>
        IEnumerable<EntityLogRecord> GetCalculatedParams(BasePersonalAccount account);

        /// <summary>
        /// Определить максимальную глубину перерасчета вызванного изменением версионируемых параметров
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="chargePeriod">Расчетный период для которого определяем глубину перерасчета</param>
        /// <returns></returns>
        Dictionary<long, DateTime> GetStartDateRecalc(IQueryable<BasePersonalAccount> accounts, ChargePeriod chargePeriod);

        /// <summary>
        /// Получить данные за указанный месяц
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="period"></param>
        void GetEntityLogsByMonth(IQueryable<BasePersonalAccount> accounts, ChargePeriod period);

        /// <summary>
        /// Включить полученнные данные по месяцам в кэш расчета
        /// </summary>
        void IncludeInCacheEntityLogsByMonth();
    }
}