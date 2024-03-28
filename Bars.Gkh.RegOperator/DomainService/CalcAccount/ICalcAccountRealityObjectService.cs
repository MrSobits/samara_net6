namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;

    /// <summary>
    /// Сервис расчетных счетов домов
    /// </summary>
    public interface ICalcAccountRealityObjectService
    {
        /// <summary>
        /// Список домов на добавление
        /// </summary>
        IDataResult ListRobjectToAdd(BaseParams baseParams);

        /// <summary>
        /// Массовое создание
        /// </summary>
        IDataResult MassCreate(BaseParams baseParams);

        /// <summary>
        /// Список для регопа
        /// </summary>
        IDataResult ListForRegop(BaseParams baseParams);
    }
}