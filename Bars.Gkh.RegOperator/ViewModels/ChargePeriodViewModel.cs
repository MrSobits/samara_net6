namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class ChargePeriodViewModel : BaseViewModel<ChargePeriod>
    {
        public override IDataResult List(IDomainService<ChargePeriod> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var closedOnly = baseParams.Params.GetAs("closedOnly", false);

            var ids = baseParams.Params.GetAs("Id", string.Empty);

            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            return domainService.GetAll()
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
                .WhereIf(closedOnly, x => x.IsClosed)
                .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.StartDate,
                        x.EndDate,
                        x.IsClosed
                    })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}