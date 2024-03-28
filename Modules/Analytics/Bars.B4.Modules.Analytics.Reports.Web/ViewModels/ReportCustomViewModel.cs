namespace Bars.B4.Modules.Analytics.Reports.Web.ViewModels
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Entities;

    /// <summary>
    /// 
    /// </summary>
    public class ReportCustomViewModel : BaseViewModel<ReportCustom>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override IDataResult List(IDomainService<ReportCustom> domainService, BaseParams baseParams)
        {
			var loadParam = GetLoadParam(baseParams);

			var data = domainService.GetAll().Filter(loadParam, Container);
			var pageData = data.Paging(loadParam).ToArray();

			var codedReportsKeys = pageData.Select(x => x.CodedReportKey);

			var codedReports = Container.Resolve<ICodedReportService>().GetAll()
                .Where(x => codedReportsKeys.Contains(x.Key))
				.GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First());

			var result = pageData
                .Select(x => new
			    {
				    x.Id,
				    x.CodedReportKey,
				    Template = "Шаблон",
				    ReportName = codedReports[x.CodedReportKey].Name
			    })
			    .AsQueryable()
			    .Order(loadParam);

			return new ListDataResult(result, data.Count());
        }
    }
}
