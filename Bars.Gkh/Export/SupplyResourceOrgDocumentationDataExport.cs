namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class SupplyResourceOrgDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /* Показывать актуальные */
            var showNotValid = baseParams.Params.ContainsKey("showNotValid") && baseParams.Params["showNotValid"].ToBool();

            return this.Container.Resolve<IDomainService<SupplyResourceOrg>>().GetAll()
                .WhereIf(!showNotValid, x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Inn,
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