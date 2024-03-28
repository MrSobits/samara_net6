namespace Bars.GisIntegration.UI.ViewModel
{
    using Bars.B4;

    /// <summary>
    /// View - модель результатов обработки объектов
    /// </summary>
    public interface IObjectProcessingResultViewModel
    {
        /// <summary>
        /// Получить список записей протокола
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// triggerId - идентификатор триггера,
        /// или
        /// packageId - идентификатор связки триггера и пакета</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий список записей протокола</returns>
        IDataResult List(BaseParams baseParams);
    }
}
