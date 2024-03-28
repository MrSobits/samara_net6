namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;

    using Entities;

    public class PeriodDiViewModel : BaseViewModel<PeriodDi>
    {
        public override IDataResult List(IDomainService<PeriodDi> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var startDate = baseParams.Params.GetAs("startDate", DateTime.MinValue);
            var endDate = baseParams.Params.GetAs("endDate", DateTime.MinValue);

            var data = domainService.GetAll()
                .WhereIf(startDate != DateTime.MinValue, x => x.DateStart >= startDate)
                .WhereIf(endDate != DateTime.MinValue, x => x.DateStart <= endDate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DateStart,
                    x.DateEnd,
                    x.DateAccounting
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}