namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Сервис для получения объектов при выполнении экспорта/импорта данных через сервис Services
    /// </summary>
    public interface IServicesService
    {
        /// <summary>
        /// Метод получения списка объектов текущего ремонта
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult GetRepairObjectList(BaseParams baseParams);

        /// <summary>
        /// Метод возвращает список активных программ текущего ремонта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult GetRepairProgramList(BaseParams baseParams);

        /// <summary>
        /// Метод возвращает список актов выполненных работ
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult GetCompletedWorkList(BaseParams baseParams);
    }
}
