namespace Bars.Gkh.Gis.DomainService.Report
{
    using B4;

    public interface IBillingReportService
    {
        /// <summary>
        /// The get print form result.
        /// </summary>
        /// <param name="baseParams">
        /// The base parameters.
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        IDataResult GetReport(BaseParams baseParams);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetExportFormatList(BaseParams baseParams);
    }
}