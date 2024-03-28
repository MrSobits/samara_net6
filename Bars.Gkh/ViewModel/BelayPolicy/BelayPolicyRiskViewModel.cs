namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BelayPolicyRiskViewModel : BaseViewModel<BelayPolicyRisk>
    {
        public override IDataResult List(IDomainService<BelayPolicyRisk> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var belayPolicyId = baseParams.Params.GetAs<long>("belayPolicyId");

            var data = domain.GetAll()
                .Where(x => x.BelayPolicy.Id == belayPolicyId)
                .Select(x => new
                {
                    x.Id,
                    x.KindRisk.Name
                }).Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}