namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectBelayPolicyViewModel : BaseViewModel<BelayPolicy>
    {
        public override IDataResult List(IDomainService<BelayPolicy> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var service = Container.Resolve<IDomainService<BelayPolicyMkd>>();

            var data = service.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.BelayPolicy.Id,
                    BelayOrganizationName = x.BelayPolicy.BelayOrganization.Contragent.Name,
                    ManagingOrganizationName = x.BelayPolicy.BelayManOrgActivity.ManagingOrganization.Contragent.Name,
                    x.BelayPolicy.DocumentDate,
                    x.BelayPolicy.DocumentNumber,
                    x.BelayPolicy.DocumentStartDate
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<BelayPolicy> domain, BaseParams baseParams)
        {
            var obj = domain.Get(baseParams.Params.GetAs<long>("id"));

            if (obj.BelayOrganization != null)
            {
                var contragent = Container.Resolve<IDomainService<Contragent>>().Get(obj.BelayOrganization.Contragent.Id);

                obj.BelayOrganization.ContragentName = contragent.Name;
            }

            return new BaseDataResult(new
            {
                obj.Id,
                BelayOrganizationName = obj.BelayOrganization != null ? obj.BelayOrganization.ContragentName : string.Empty,
                ManagingOrganizationName = obj.BelayManOrgActivity.ManagingOrganization.Contragent.Name,
                obj.DocumentNumber,
                obj.DocumentDate,
                obj.DocumentStartDate,
                obj.DocumentEndDate,
                obj.LimitManOrgInsured,
                obj.LimitCivilOne,
                obj.Cause,
                obj.PolicyAction
            });
        }
    }
}