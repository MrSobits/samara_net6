namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для работы с ограничениями на выбор скорректированного года
    /// </summary>
    public interface IYearCorrectionConfigService
    {
        /// <summary>
        /// Метод проверяет доступность указанного года корректировки из единых настроек
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Результат проверки</returns>
        IDataResult IsValidYear(int year);
    }
}