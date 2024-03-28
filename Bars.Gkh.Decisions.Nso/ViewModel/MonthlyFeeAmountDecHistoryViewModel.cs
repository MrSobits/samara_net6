namespace Bars.Gkh.Decisions.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class MonthlyFeeAmountDecHistoryViewModel : BaseViewModel<MonthlyFeeAmountDecHistory>
    {
        public override IDataResult List(IDomainService<MonthlyFeeAmountDecHistory> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realityObjId = baseParams.Params.GetAsId("realityObjId");

            var result = domainService.GetAll()
                .Where(x => x.Protocol.RealityObject.Id == realityObjId)
                .AsEnumerable()
                .SelectMany(x => x.Decision.Select(y => new
                {
                    x.Protocol.ProtocolDate,
                    y.From,
                    y.To,
                    y.Value,
                    x.UserName,
                    x.ObjectCreateDate
                })
                .AsEnumerable())
                .AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.ObjectCreateDate);

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
        }
    }
}