namespace Bars.Gkh.Decisions.Nso.Domain
{
    using B4;
    using Bars.Gkh.Enums.Decisions;
    using Entities;
    using Entities.Decisions;
    using Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Сервис Протокол решений собственников
    /// </summary>
    public interface IRealityObjectDecisionsService
    {
        /// <summary>
        /// Возвращает список домов с решением формирования фонда дома на специальном счете.
        /// </summary>
        /// <returns></returns>
        ListDataResult RealityObjectsOnSpecialAccount();

        /// <summary>
        /// Возвращает список домов с решением формирования фонда дома у регоператора.
        /// </summary>
        /// <returns></returns>
        ListDataResult RealityObjectsOnRegOpAccount();

        /// <summary>
        /// Возвращает список домов с решением формирования фонда дома определенного типа.
        /// </summary>
        /// <param name="type">Тип решения по формированию дома</param>
        /// <returns></returns>
        ListDataResult RealityObjectsByCrFundDecisionType(CrFundFormationDecisionType type);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ListDataResult RealityObjectCreditOrgDecisions();

        /// <summary>
        /// Возвращает актуальное решение на доме
        /// </summary>
        /// <typeparam name="T">Тип решения</typeparam>
        /// <param name="realityObject"></param>
        /// <param name="inFinalState">Статус конечный</param>
        /// <param name="protocolsToFilter">При проверке пропускать эти протоколы</param>
        /// <returns>Решение</returns>
        T GetActualDecision<T>(RealityObject realityObject, bool inFinalState = false, IEnumerable<RealityObjectDecisionProtocol> protocolsToFilter = null, DateTime? date = null) where T : UltimateDecision;

        /// <summary>
        /// Возвращает список актуальное решение на доме
        /// </summary>
        /// <typeparam name="T">Тип решения</typeparam>
        /// <param name="realityObject">Жилой дом</param>
        /// <param name="protocolsToFilter">При проверке пропускать эти протоколы</param>
        List<T> GetActualDecisions<T>(RealityObject realityObject,  IEnumerable<RealityObjectDecisionProtocol> protocolsToFilter = null) where T : UltimateDecision;

        /// <summary>
        /// Возвращает словарь актуальное решение по домам 
        /// </summary>
        /// <typeparam name="T">Тип решения</typeparam>
        /// <param name="realityObjectIds">Жилой дом</param>
        Dictionary<long, IGrouping<long, T>> GetActualDecisions<T>(List<long> realityObjectIds, DateTime? date = null) where T : UltimateDecision;

        /// <summary>
        /// Возвращает словарь актуальных решений по домам
        /// </summary>
        /// <typeparam name="T">Тип решения</typeparam>
        /// <param name="ros">Жилые дома</param>
        /// <param name="inFinalState">Флаг для выбора только конечного статуса </param>
        /// <returns></returns>
        Dictionary<RealityObject, T> GetActualDecisionForCollection<T>(IEnumerable<RealityObject> ros, bool inFinalState) where T : UltimateDecision;

        /// <summary>
        /// Подсчет периодов действия способа формирования фонда
        /// </summary>
        /// <param name="roIds"></param>
        /// <returns></returns>
        Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> GetRobjectsFundFormation(IQueryable<long> roIds);

        /// <summary>
        /// Подсчет периодов действия способа формирования фонда
        /// </summary>
        /// <param name="roIds"></param>
        /// <returns></returns>
        Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> GetRobjectsFundFormationForRecalc(ICollection<long> roIds = null);

        /// <summary>
        /// Подсчет периодов действия способа формирования фонда
        /// </summary>
        /// <param name="roIds"></param>
        /// <returns></returns>
        Dictionary<long, IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>>> GetRobjectsFundFormation(ICollection<long> roIds = null);

        /// <summary>
        /// Получение протокол решения органа государственной власти по дому
        /// </summary>
        /// <param name="robject">Жилой дом</param>
        /// <returns></returns>
        GovDecision GetCurrentGovDecision(RealityObject robject);
    }
}