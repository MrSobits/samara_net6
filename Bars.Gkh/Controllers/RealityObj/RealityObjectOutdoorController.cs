namespace Bars.Gkh.Controllers.RealityObj
{
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService.RealityObjectOutdoor;
    using Bars.Gkh.Entities.RealityObj;
    using Microsoft.AspNetCore.Mvc;

    public class RealityObjectOutdoorController : B4.Alt.DataController<RealityObjectOutdoor>
    {
        public ActionResult UpdateOutdoorInRealityObjects(BaseParams baseParams) =>
            this.Container.Resolve<IRealityObjectOutdoorService>().UpdateOutdoorInRealityObjects(baseParams);

        public ActionResult DeleteOutdoorFromRealityObject(BaseParams baseParams) =>
            this.Container.Resolve<IRealityObjectOutdoorService>().DeleteOutdoorFromRealityObject(baseParams);

        public ActionResult Export(BaseParams baseParams) =>
            this.Container.Resolve<IDataExportService>("RealityObjectOutdoorDataExport").ExportData(baseParams);
    }
}
