namespace Bars.Gkh.Modules.RegOperator.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.Controller;

    /// <summary>
    /// Интерфейс сервиса лс для печати отчетов
    /// </summary>
    public interface IPersonalAccountReportService
    {
        /// <summary>
        /// Отчет по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetReport(BaseParams baseParams);

        /// <summary>
        /// Отчет по многим лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetMassReport(BaseParams baseParams);

        /// <summary>
        /// Получить все отчеты по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        ReportInfo GetReportInfo(BaseParams baseParams);
    }
}