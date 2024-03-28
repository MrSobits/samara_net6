namespace Bars.GkhEdoInteg.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhEdoInteg.Entities;

    public class RevenueSourceCompareEdoViewModel : BaseViewModel<RevenueSourceCompareEdo>
    {
        public override IDataResult List(IDomainService<RevenueSourceCompareEdo> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var data = domainService
                .GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.RevenueSource.Name,
                    x.RevenueSource.Code,
                    x.RevenueSource,
                    x.CodeEdo
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
