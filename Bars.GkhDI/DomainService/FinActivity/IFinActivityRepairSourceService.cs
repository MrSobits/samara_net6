namespace Bars.GkhDi.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис для обработки данных по Объему привлеченных средств на ремонт и благоустройство
    /// </summary>
    public interface IFinActivityRepairSourceService
    {
        /// <summary>
        /// Добавить данные
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult AddWorkMode(BaseParams baseParams);

        /// <summary>
        /// Добавить данные из домов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult AddDataByRealityObj(BaseParams baseParams);
    }
}