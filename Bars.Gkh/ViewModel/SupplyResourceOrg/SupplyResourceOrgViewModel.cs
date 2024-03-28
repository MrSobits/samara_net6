namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class SupplyResourceOrgViewModel : BaseViewModel<SupplyResourceOrg>
    {
        public override IDataResult List(IDomainService<SupplyResourceOrg> domain, BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var activeOperator = userManager.GetActiveOperator();
            var contragent1468 = activeOperator != null ? activeOperator.Contragent : null;
            
            var loadParams = GetLoadParam(baseParams);

            /* Показывать актуальные */
            var showNotValid = baseParams.Params.ContainsKey("showNotValid") && baseParams.Params["showNotValid"].ToBool();

            var data = domain.GetAll()
                .WhereIf(!showNotValid, x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .WhereIf(contragent1468 != null, x => x.Contragent.Id == contragent1468.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Inn,
                    ContragentId = x.Contragent.Id,
                    ContragentName = x.Contragent.Name,
                    x.ActivityGroundsTermination,
                    Municipality = x.Contragent.Municipality.Name,
                    Settlement = x.Contragent.MoSettlement.Name
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}