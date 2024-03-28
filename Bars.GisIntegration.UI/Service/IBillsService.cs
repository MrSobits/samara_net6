namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для получения объектов при выполнении импорта/экспорта данных через сервис Bills
    /// </summary>
    public interface IBillsService
    {
        /// <summary>
        /// Метод возвращает запросы на проведения квитирования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список запросов</returns>
        IDataResult GetAcknowledgments(BaseParams baseParams);
    }
}
