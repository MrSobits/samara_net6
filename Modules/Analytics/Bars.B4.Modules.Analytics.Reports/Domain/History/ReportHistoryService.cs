namespace Bars.B4.Modules.Analytics.Reports.Domain.History
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Reports.Entities.History;

    /// <summary>
    /// Сервис для работы с историей печати отчетов
    /// </summary>
    public class ReportHistoryService : IReportHistoryService
    {
        public IDomainService<ReportHistory> ReportHistoryDomain { get; set; }

        /// <inheritdoc />
        public IDataResult ReportHistoryParamList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var historyId = loadParams.Filter.GetAs<long?>("historyId") ?? baseParams.Params.GetAs<long>("historyId");
            var history = this.ReportHistoryDomain.Get(historyId);    

            var result = history.ParameterValues
                .Select(
                    y => new
                    {
                        Name = y.Key,
                        y.Value.Value,
                        y.Value.DisplayName,
                        y.Value.DisplayValue
                    })
                .ToList();

            return new ListDataResult(result, result.Count);                 
        }
    }
}