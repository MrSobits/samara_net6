namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс детализации по зачету средств за ранее проделанные работы
    /// </summary>
    public interface IPerformedWorkDetailService
    {
        /// <summary>
        /// Вернуть детализацию распределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult ListDistributed(BaseParams baseParams);

        /// <summary>
        /// Метод возвращает предполагаемые распределения на текущий период для детализации
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns></returns>
        IList<PeriodOperationDetail> GetSupposedDistributions(BasePersonalAccount account);

        /// <summary>
        /// Вернуть планируемую сумму зачёта средств
        /// </summary>
        /// <param name="accountQuery">Запрос по лицевым счетам</param>
        /// <param name="connection">Соединение с бд, если передано, то будет использоваться именно оно, вместо NHibernateSession</param>
        /// <returns>Словарь сумм</returns>
        IDictionary<long, Tuple<decimal, decimal>> GetResultDistributionSum(IQueryable<BasePersonalAccount> accountQuery, IDbConnection connection = null);
    }
}