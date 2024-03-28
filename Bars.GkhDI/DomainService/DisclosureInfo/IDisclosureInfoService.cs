namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Интерфейс сервиса по расчёту процентов
    /// </summary>
    public interface IDisclosureInfoService
    {
        /// <summary>
        /// Получить объект <see cref="DisclosureInfoRealityObj"/>
        /// </summary>
        DisclosureInfoRealityObj GetDiRoByDi(long disclosureInfoId, long realityObjId, long manOrgId);

        /// <summary>
        /// Получить прокси <see cref="DisclosureInfoRealityObj"/>
        /// </summary>
        /// <param name="disclosureInfoId"><see cref="DisclosureInfo"/></param>
        /// <param name="realityObjId"><see cref="RealityObject"/></param>
        DisclosureInfoRealityObj GetDiRoProxyByDi(long disclosureInfoId, long realityObjId);

        /// <summary>
        /// Получить DisclosureInfo
        /// </summary>
        IDataResult GetDisclosureInfo(BaseParams baseParams);

        /// <summary>
        /// Получить OperatorManOrg
        /// </summary>
        IDataResult GetOperatorManOrg(BaseParams baseParams);

        /// <summary>
        /// Получить DateStartByPeriod
        /// </summary>
        IDataResult GetDateStartByPeriod(BaseParams baseParams);

        /// <summary>
        /// Получить TypeManagingByDisinfo
        /// </summary>
        IDataResult GetTypeManagingByDisinfo(BaseParams baseParams);

        /// <summary>
        /// Получить DisclosureOfManOrg
        /// </summary>
        IDataResult GetDisclosureOfManOrg(BaseParams baseParams);

        /// <summary>
        /// Получить PositionByCode
        /// </summary>
        string GetPositionByCode(long contragentId, PeriodDi periodDi, List<string> listCodes);

        /// <summary>
        /// Выполнить задачу по расчету процентов согласно настроек
        /// </summary>
        IDataResult PercentCalculation(BaseParams baseParams);

        /// <summary>
        /// Рассчитать проценты
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат расчёта</returns>
        IDataResult StartPercentCalculation(BaseParams baseParams);
    }
}