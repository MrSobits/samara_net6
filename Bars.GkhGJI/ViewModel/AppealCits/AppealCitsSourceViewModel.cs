namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class AppealCitsSourceViewModel : BaseViewModel<AppealCitsSource>
    {
        public override IDataResult List(IDomainService<AppealCitsSource> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            var data = domainService.GetAll()
                .WhereIf(appealCitizensId > 0, x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                    {
                        x.Id,
                        x.AppealCits,
                        x.RevenueDate,
                        x.SSTUDate,
                        x.RevenueSourceNumber,
                        RevenueSource = x.RevenueSource != null ? x.RevenueSource.Name : string.Empty,
                        RevenueForm = x.RevenueForm != null ? x.RevenueForm.Name : string.Empty
                    })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}