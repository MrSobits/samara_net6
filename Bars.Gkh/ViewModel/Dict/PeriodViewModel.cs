namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class PeriodViewModel : BaseViewModel<Period>
    {
        public override IDataResult List(IDomainService<Period> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var ids = baseParams.Params.GetAs("Id", string.Empty);

            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var data = domain.GetAll()
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DateStart,
                    x.DateEnd
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        } 
    }
}