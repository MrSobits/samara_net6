namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    
    /// <summary>
    /// Сервис для "Период подачи тепла"
    /// </summary>
    public interface IHeatInputService
    {
        /// <summary>
        /// Информация по болйлерам 
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult GetBoilerInfo(BaseParams baseParams);

        /// <summary>
        /// Сохранение информации "Период подачи тепла"
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult SaveHeatInputInfo(BaseParams baseParams);

        IDataResult SaveWorkWinterInfo(BaseParams baseParams);

        /// <summary>
        /// Копирование данных из периода
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult CopyPeriodWorkWinterCondition(BaseParams baseParams);
    }
}