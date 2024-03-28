namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Сервис работы оплатами
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Вернуть список распоряжений, помеченных к удалению из ГИСа
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetNotificationsToDelete(BaseParams baseParams);


        /// <summary>
        /// Получить список распоряжений
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetNotificationsToAdd(BaseParams baseParams);
    }
}