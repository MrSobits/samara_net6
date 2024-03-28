using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;

    public class TatarstanProtocolMvdDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITatarstanProtocolMvdService>();
            var loadParam = baseParams.GetLoadParam();
            var res = service.GetList(baseParams, true).AsQueryable().Filter(loadParam, this.Container).Order(loadParam).ToList();
            this.Container.Release(service);

            return res;
        }
    }
}
