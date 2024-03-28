namespace Bars.Gkh.Reforma.Domain
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с выборочной интеграцией Реформы.ЖКХ
    /// </summary>
    public interface IManualIntegrationService
    {
        /// <summary>
        /// Добавить в очередь задачу интеграции УО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult ScheduleManorgIntegrationTask(BaseParams baseParams);

        /// <summary>
        /// Добавить в очередь задачу интеграции домов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult ScheduleRobjectIntegrationTask(BaseParams baseParams);

        /// <summary>
        /// Вернуть список управляемых домов для выполнения выборочной интеграции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операци</returns>
        IDataResult ListManagedRealityObjects(BaseParams baseParams);
    }
}