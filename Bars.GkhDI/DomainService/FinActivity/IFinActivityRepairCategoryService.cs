namespace Bars.GkhDi.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис для обработки данных по Ремонту дома и благоустройству территории, средний срок обслуживания МКД
    /// </summary>
    public interface IFinActivityRepairCategoryService
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