namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class BelayPolicyViewModel : BaseViewModel<BelayPolicy>
    {
        public override IDataResult List(IDomainService<BelayPolicy> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var belayManOrgActivId = baseParams.Params.GetAs<long>("belayManOrgActivId");

            var data = domain.GetAll()
                .Where(x => x.BelayManOrgActivity.Id == belayManOrgActivId)
                .Select(x => new
                    {
                        x.Id,
                        ContragentName = x.BelayOrganization.Contragent.Name,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentStartDate
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<BelayPolicy> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params["id"].To<long>());

            return new BaseDataResult(new
                {
                    obj.Id,
                    BelayManOrgActivity = obj.BelayManOrgActivity.Id,
                    BelayOrganization = new { obj.BelayOrganization.Id, ContragentName = obj.BelayOrganization.Contragent.Name},
                    obj.BelayOrgKindActivity,
                    obj.BelaySum,
                    obj.Cause,
                    obj.DocumentDate,
                    obj.DocumentEndDate,
                    obj.DocumentNumber,
                    obj.DocumentStartDate,
                    obj.LimitCivil,
                    obj.LimitCivilInsured,
                    obj.LimitCivilOne,
                    obj.LimitManOrgHome,
                    obj.LimitManOrgInsured,
                    obj.PolicyAction
                });
        }
    }
}