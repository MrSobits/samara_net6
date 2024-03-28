namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Интерфейс работы с расщеплением платежей
    /// </summary>
    public interface IChargeSplittingService
    {
        /// <summary>
        /// Сформировать записи за период
        /// </summary>
        /// <param name="period">Отчетный период</param>
        /// <returns>Успешность</returns>
        IDataResult CreateSummaries(ContractPeriod period);

        /// <summary>
        /// Пересчитать суммы
        /// </summary>
        /// <param name="periodSumm">Информация за период</param>
        /// <returns>Успешность</returns>
        IDataResult RecalcSummary(PubServContractPeriodSumm periodSumm);

        /// <summary>
        /// Актуализировать сведения за период
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        IDataResult ActualizeSummaries(BaseParams baseParams);

        /// <summary>
        /// Актуализировать сведения по договорам ТЭР
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult ActualizeFuelEnergyValues(BaseParams baseParams);
    }

    /// <summary>
    /// Интерфейс экспорта договоров расщепления (УО)
    /// </summary>
    public interface IPublicServiceOrgExportService
    {
        /// <summary>
        /// Экспорт
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>report</returns>
        ReportStreamResult ExportToCsv(BaseParams baseParams);
    }

    /// <summary>
    /// Интерфейс экспорта договоров расщепления (УО)
    /// </summary>
    public interface IBudgetOrgContractExportService
    {
        /// <summary>
        /// Экспорт
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>report</returns>
        ReportStreamResult ExportToCsv(BaseParams baseParams);
    }
}