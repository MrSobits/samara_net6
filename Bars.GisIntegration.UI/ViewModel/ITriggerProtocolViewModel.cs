namespace Bars.GisIntegration.UI.ViewModel
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс View - модели протокола выполнения триггера
    /// </summary>
    public interface ITriggerProtocolViewModel
    {
        /// <summary>
        /// Получить список записей протокола
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// triggerId - идентификатор триггера,
        /// sendDataProtocol - признак, характеризующий тип триггера</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий список записей протокола</returns>
        IDataResult List(BaseParams baseParams);
    }
}
