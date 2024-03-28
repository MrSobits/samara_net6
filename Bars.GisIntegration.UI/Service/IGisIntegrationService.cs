namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;
    using Bars.GisIntegration.Base.Entities;

    public interface IGisIntegrationService
    {
        /// <summary>
        /// Получить список контрагентов привязанных к текущему пользователю
        /// или делегировавших ему полномочия
        /// </summary>
        IDataResult ListContragents(BaseParams baseParams);
        
        /// <summary>
        /// Получить список методов
        /// </summary>
        IDataResult GetMethodList(BaseParams baseParams);


        /// <summary>
        /// Проверить выполнимость метода
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор метода</param>
        /// <returns>Результат проверки</returns>
        IDataResult CheckMethodFeasibility(BaseParams baseParams);

        /// <summary>
        ///  Запланировать подготовку данных: извлечение, валидация, формирование пакетов
        /// </summary>
        /// <param name="baseParams">Параметры подготовки данных, содержащие фильтры, идентификатор метода</param>
        /// <returns>Результат планирования</returns>
        IDataResult SchedulePrepareData(BaseParams baseParams);

        /// <summary>
        ///  Запланировать отправку данных
        /// </summary>
        /// <param name="baseParams">Параметры отправки данных, содержащие
        /// идентификатор задачи,
        /// идентификаторы пакетов к отправке</param>
        /// <returns>Результат планирования</returns>
        IDataResult ScheduleSendData(BaseParams baseParams);

        /// <summary>
        /// Получить параметры выполнения подписывания и отправки данных
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор задачи</param>
        /// <returns>Параметры выполнения подписывания и отправки данных</returns>
        IDataResult GetSignAndSendDataParams(BaseParams baseParams);
    }
}