namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class FinanceSourceWorkViewModel : BaseViewModel<FinanceSourceWork>
    {
        public override IDataResult List(IDomainService<FinanceSourceWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var financeSourceId = baseParams.Params.ContainsKey("financeSourceId")
                        ? baseParams.Params["financeSourceId"].ToLong()
                        : 0L;

            var data = domainService.GetAll()
                .Where(x => x.FinanceSource.Id == financeSourceId)
                .Select(x => new
                {
                    x.Id,
                    WorkName = x.Work.Name
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}
