namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class BelayPolicyMkdViewModel : BaseViewModel<BelayPolicyMkd>
    {
        public override IDataResult List(IDomainService<BelayPolicyMkd> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var belayPolicyId = baseParams.Params.GetAs<long>("belayPolicyId");

            var isExcluded = baseParams.Params.GetAs("isExcluded", false);

            var data = domain.GetAll()
                .Where(x => x.BelayPolicy.Id == belayPolicyId && x.IsExcluded == isExcluded)
                .Select(x => new
                    {
                        x.Id,
                        x.RealityObject.Address,
                        IsExcluded = x.IsExcluded ? "Да" : "Нет"
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}