namespace Bars.B4.Modules.Analytics.Reports.ViewModels.History
{
    using System.Linq;
    using Bars.B4.Utils;

    using Entities.History;

    public class ReportHistoryViewModel : BaseViewModel<ReportHistory>
    {
        public IUserInfoProvider UserInfoProvider { get; set; }

        public override IDataResult List(IDomainService<ReportHistory> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var useUserFilter = baseParams.Params.GetAs<bool>("useUserFilter");

            long userId = this.UserInfoProvider?.GetActiveUser().Id ?? 0;

            var data = domainService.GetAll()
                .WhereIf(useUserFilter, x => x.User.Id == userId)
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    x.Name,
                    Category = x.Category.Name,
                    x.ReportType,
                    x.File,
                    x.ReportId
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Date)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}