namespace Bars.B4.Modules.Analytics.Reports.ViewModels
{
    using System.Linq;

    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Utils;

    public class PrintFormCategoryViewModel : BaseViewModel<PrintFormCategory>
    {
        public override IDataResult List(IDomainService<PrintFormCategory> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var usePaging = baseParams.Params.GetAs<bool>("usePaging", true);

            var data = domainService.GetAll()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Id)
                .Filter(loadParams, this.Container);

            if (usePaging)
            {
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }

            return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
        }
    }
}