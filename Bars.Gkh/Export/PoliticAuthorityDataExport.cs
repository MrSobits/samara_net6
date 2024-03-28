namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class PoliticAuthorityDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            return Container.Resolve<IDomainService<PoliticAuthority>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.Contragent.Name,
                    x.Contragent.Inn,
                    Municipality = x.Contragent.Municipality.Name,
                    x.NameDepartamentGkh,
                    x.OfficialSite
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}