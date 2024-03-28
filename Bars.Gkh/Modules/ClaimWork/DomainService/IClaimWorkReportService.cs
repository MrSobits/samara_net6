namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using System.Collections.Generic;

    using B4;

    using Bars.Gkh.Modules.ClaimWork.Controller;

    /// <summary>
    /// Сервис для работы с претензиями неплательщиков
    /// </summary>
    public interface IClaimWorkReportService
    {
        /// <summary>
        /// Получить список сущностей для которых реализован IClaimWorkCodedReport + отчет по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IList<ReportInfo> GetReportList(BaseParams baseParams);

        /// <summary>
        /// Создание и вывод одной печатной формы
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetReport(BaseParams baseParams);

        /// <summary>
        /// Массовое создание печатных форм и сохранение на ftp
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetMassReport(BaseParams baseParams);

        /// <summary>
        /// Создание и вывод одной печатной формы
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetLawsuitOnwerReport(BaseParams baseParams);

        /// <summary>
        /// Создание и вывод печатной формы из списка ЛС
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetAccountReport(BaseParams baseParams);

        /// <summary>
        /// Массовое создание печатных форм по ЛС
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetMassAccountReport(BaseParams baseParams);
    }
}