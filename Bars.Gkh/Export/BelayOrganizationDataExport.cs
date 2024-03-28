namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class BelayOrganizationDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var showNotValid = baseParams.Params.ContainsKey("showNotValid") && baseParams.Params["showNotValid"].ToBool();

            return Container.Resolve<IDomainService<BelayOrganization>>().GetAll()
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