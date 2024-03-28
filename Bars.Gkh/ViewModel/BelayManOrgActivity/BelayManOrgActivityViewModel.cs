namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class BelayManOrgActivityViewModel : BaseViewModel<BelayManOrgActivity>
    {
        public override IDataResult List(IDomainService<BelayManOrgActivity> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        ContragentName = x.ManagingOrganization.Contragent.Name,
                        ContragentInn = x.ManagingOrganization.Contragent.Inn,
                        Municipality = x.ManagingOrganization.Contragent.Municipality.Name
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<BelayManOrgActivity> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params["id"].To<long>());

            return new BaseDataResult(new
                {
                    obj.Id,
                    ManagingOrganization = new { obj.ManagingOrganization.Id, ContragentName = obj.ManagingOrganization.Contragent.Name }
                });
        }
    }
}