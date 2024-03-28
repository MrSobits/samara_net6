namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для получения объектов при выполнении импорта/экспорта данных через сервис HouseManagement
    /// </summary>
    public interface IHouseManagementService
    {
        /// <summary>
        /// Получить список домов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetHouseList(BaseParams baseParams);

        /// <summary>
        /// Получить список договоров
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetContractList(BaseParams baseParams);

        /// <summary>
        /// Получить список уставов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetCharterList(BaseParams baseParams);

        /// <summary>
        /// Получить список ДОИ
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetPublicPropertyContractList(BaseParams baseParams);

        /// <summary>
        /// Получить список новостей
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetNotificationList(BaseParams baseParams);

        /// <summary>
        /// Получить список договоров ресурсоснабжения
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult GetSupplyResourceContractList(BaseParams baseParams);
    }
}
