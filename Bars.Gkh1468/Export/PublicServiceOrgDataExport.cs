namespace Bars.Gkh1468.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class PublicServiceOrgDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var userManager = Container.Resolve<IGkhUserManager>();
            var contragentList = userManager.GetContragentIds();
            var activeOperator = userManager.GetActiveOperator();
            var contragent1468Id = activeOperator != null && activeOperator.Contragent != null ? activeOperator.Contragent.Id : 0;

            var operatorHasContragent = baseParams.Params.ContainsKey("operatorHasContragent") && baseParams.Params["operatorHasContragent"].ToBool();

            var data = this.Container.Resolve<IDomainService<PublicServiceOrg>>().GetAll();

            if (operatorHasContragent)
            {
                data = data.WhereIf(contragentList.Count > 0 || contragent1468Id > 0, x => contragentList.Contains(x.Contragent.Id) || contragent1468Id == x.Contragent.Id);
            }

            return data
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Inn,
                    ContragentId = x.Contragent.Id,
                    ContragentName = x.Contragent.Name,
                    x.ActivityGroundsTermination,
                    Municipality = x.Contragent.Municipality.Name
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}