namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class BelayManOrgActivityDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<BelayManOrgActivity>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.ManagingOrganization.Contragent.Name,
                    ContragentInn = x.ManagingOrganization.Contragent.Inn,
                    Municipality = x.ManagingOrganization.Contragent.Municipality.Name
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}