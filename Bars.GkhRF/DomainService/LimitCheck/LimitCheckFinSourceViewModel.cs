namespace Bars.GkhRf.DomainService
{
    using B4;
    using B4.Utils;
    using Entities;

    using System.Linq;

    public class LimitCheckFinSourceViewModel : BaseViewModel<LimitCheckFinSource>
    {
        public override IDataResult List(IDomainService<LimitCheckFinSource> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var limitCheckId = baseParams.Params.ContainsKey("limitCheckId")
                                   ? baseParams.Params["limitCheckId"].ToLong()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.LimitCheck.Id == limitCheckId)
                .Select(x => new
                    {
                        x.Id,
                        FinanceSource = x.FinanceSource.Name
                    })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}