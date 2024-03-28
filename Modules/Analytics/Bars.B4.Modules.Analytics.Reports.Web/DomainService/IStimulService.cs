namespace Bars.B4.Modules.Analytics.Reports.Web.DomainService
{
    /// <summary>
    /// Интерфейс для генерации стимул отчетов
    /// </summary>
    public interface IStimulService
    {
        /// <summary>
        /// Сохранение шаблона
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        void SaveTemplate(BaseParams baseParams);
    }
}
