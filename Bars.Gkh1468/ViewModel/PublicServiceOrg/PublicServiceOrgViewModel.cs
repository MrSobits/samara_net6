namespace Bars.Gkh1468.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class PublicServiceOrgViewModel : BaseViewModel<PublicServiceOrg>
    {
        public override IDataResult List(IDomainService<PublicServiceOrg> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var userManager = Container.Resolve<IGkhUserManager>();
            var contragentList = userManager.GetContragentIds();
            var activeOperator = userManager.GetActiveOperator();
            var contragent1468Id = activeOperator != null && activeOperator.Contragent != null ? activeOperator.Contragent.Id : 0;

            var operatorHasContragent = baseParams.Params.ContainsKey("operatorHasContragent") && baseParams.Params["operatorHasContragent"].ToBool();

            /* Показывать актуальные */
            //var showNotValid = baseParams.Params.ContainsKey("showNotValid") && baseParams.Params["showNotValid"].ToBool();

            var data = domain.GetAll();

            if (operatorHasContragent)
            {
                data = data.WhereIf(contragentList.Count > 0 || contragent1468Id > 0, x => contragentList.Contains(x.Contragent.Id) || contragent1468Id == x.Contragent.Id);
            }

            var result = data
                //.WhereIf(!showNotValid, x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .Select(x => new
                    {
                        x.Id,
                        x.Contragent.Inn,
                        ContragentId = x.Contragent.Id,
                        ContragentName = x.Contragent.Name,
                        x.ActivityGroundsTermination,
                        Municipality = x.Contragent.Municipality.Name
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            int totalCount = result.Count();

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}