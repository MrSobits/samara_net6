namespace Bars.Gkh.DomainService
{
    using B4;

    /// <summary>
    /// Интерфейс сервиса "Управление домами (ТСЖ / ЖСК)"
    /// </summary>
    public interface IManOrgJskTsjContractService
    {
        /// <summary>
        /// Возвращает объект недвижимости договора
        /// </summary>
        /// <returns>Жилой дом</returns>
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Проверка на дату договора управления 
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат проверки</returns>
        IDataResult VerificationDate(BaseParams baseParams);

    }
}
