namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Entities;

    public interface ICalcAccountService
    {
        Dictionary<long, CalcAccountSummaryProxy> GetAccountSummary(IQueryable<CalcAccountRealityObject> query);

        Dictionary<long, CalcAccountSummaryProxy> GetAccountSummary(CalcAccount account);

        IDataResult GetRegopAccountSummary(BaseParams baseParams);

        IDataResult ListOperations(BaseParams baseParams);
        IDataResult ListOperationsSum(BaseParams baseParams);

        /// <summary>
        /// Возвращает расчетный счет указанного типа, к которому привязан дом
        /// в случае отсутствия такого типа счета, вернет null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ro"></param>
        /// <returns></returns>
        T GetRobjectAccount<T>(RealityObject ro) where T : CalcAccount;

        /// <summary>
        /// Возвращает словарь соответствия жилого дома и расчетного счета, к которому дом привязан
        /// </summary>
        /// <param name="query">фильтр жилых домов IQueryable[RealityObject]</param>
        /// <param name="date"></param>
        /// <returns></returns>
        Dictionary<long, CalcAccount> GetRobjectsAccounts(IQueryable<RealityObject> query = null, DateTime date = new DateTime());

        /// <summary>
        /// Возвращает словарь соответствия жилого дома и расчетных счетов, к которым привязан дом
        /// </summary>
        Dictionary<long, List<CalcAccount>> GetRobjectsAllAccounts(IQueryable<RealityObject> query, DateTime date);

        /// <summary>
        /// Возвращает словарь соответствия жилого дома и расчетного счета, к которому дом привязан
        /// </summary>
        /// <param name="roIdsQuery">фильтр жилых домов IQueryable[RealityObject]</param>
        /// <param name="date"></param>
        /// <returns></returns>
        Dictionary<long, CalcAccount> GetRobjectsAccounts(IQueryable<long> roIdsQuery = null, DateTime date = new DateTime());

        /// <summary>
        /// Возвращает словарь соответствия жилого дома и расчетного счета, к которому дом привязан
        /// </summary>
        /// <param name="roIds">фильтр жилых домов long[]</param>
        /// <param name="date"></param>
        /// <returns></returns>
        Dictionary<long, CalcAccount> GetRobjectsAccounts(long[] roIds = null, DateTime date = new DateTime());
    }
}